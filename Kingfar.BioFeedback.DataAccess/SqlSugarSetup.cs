using Microsoft.Extensions.Configuration;
using SqlSugar;
using System.Reflection;
using Mapster;
using Kingfar.BioFeedback.Entites;
using Kingfar.BioFeedback.Shared;
using Prism.Ioc;
using Kingfar.BioFeedback.Entities.Entities;
using System.Collections;
using Kingfar.BioFeedback.Shared.Abstractions.Common;

namespace Kingfar.BioFeedback.DataAccess
{
    public static class SqlSugarSetup
    {
        public static void AddSqlSugar(this IContainerRegistry containerRegistry, IConfiguration configuration)
        {
            var dbOptions = configuration.GetSection(SqlSugarConst.DbSectionKey).Get<DbConnectionOptions>();
            dbOptions!.ConnectionConfigs.ForEach(SetDbConfig);

            SqlSugarScope sqlSugar = new(dbOptions.ConnectionConfigs.Adapt<List<ConnectionConfig>>(), db =>
            {
                dbOptions.ConnectionConfigs.ForEach(config =>
                {
                    SqlSugarScopeProvider dbProvider = db.GetConnectionScope(config.ConfigId);
                    dbProvider.SetDbAop();
                });
            });
            containerRegistry.RegisterSingleton<ISqlSugarClient>(() => sqlSugar); // 单例注册
            containerRegistry.RegisterScoped(typeof(ISqlSugarRepository<>), typeof(SqlSugarRepository<>)); // 仓储注册

            // 初始化数据库表结构及种子数据
            dbOptions.ConnectionConfigs.ForEach(config =>
            {
                sqlSugar.InitMainDatabase(config);
            });
        }

        /// <summary>
        /// 配置连接属性
        /// </summary>
        /// <param name="config"></param>
        public static void SetDbConfig(this Shared.DbConnectionConfig config)
        {
            var dbFilePath = Path.Combine(FilePathConst._dbPath, config.DbFilePath);
            if (!File.Exists(dbFilePath))
            {
                var directoryPath = Path.GetDirectoryName(dbFilePath);
                if (!Directory.Exists(directoryPath) && directoryPath is not null) Directory.CreateDirectory(directoryPath);
            }
            var configureExternalServices = new ConfigureExternalServices
            {
                EntityNameService = (type, entity) => // 处理表
                {
                    entity.IsDisabledDelete = true; // 禁止删除非 sqlsugar 创建的列
                                                    // 只处理贴了特性[SugarTable]表
                    if (!type.GetCustomAttributes<SugarTable>().Any())
                        return;
                    if (config.DbSettings.EnableUnderLine && !entity.DbTableName.Contains('_'))
                        entity.DbTableName = UtilMethods.ToUnderLine(entity.DbTableName); // 驼峰转下划线
                },
                EntityService = (type, column) => // 处理列
                {
                    // 只处理贴了特性[SugarColumn]列
                    if (!type.GetCustomAttributes<SugarColumn>().Any())
                        return;
                    if (column.IsPrimarykey == false && new NullabilityInfoContext().Create(type).WriteState is NullabilityState.Nullable)
                        column.IsNullable = true;
                    if (config.DbSettings.EnableUnderLine && !column.IsIgnore && !column.DbColumnName.Contains('_'))
                        column.DbColumnName = UtilMethods.ToUnderLine(column.DbColumnName); // 驼峰转下划线

                    if (config.DbType == SqlSugar.DbType.Oracle)
                    {
                        if (type.PropertyType == typeof(long) || type.PropertyType == typeof(long?))
                            column.DataType = "number(18)";
                        if (type.PropertyType == typeof(bool) || type.PropertyType == typeof(bool?))
                            column.DataType = "number(1)";
                    }
                },
                DataInfoCacheService = new SqlSugarCache(),
            };
            //TODO 追加密码进行数据库保护
            config.ConnectionString = "DataSource=" + dbFilePath;
            config.ConfigureExternalServices = configureExternalServices;
            config.InitKeyType = InitKeyType.Attribute;
            config.IsAutoCloseConnection = true;
            config.MoreSettings = new ConnMoreSettings
            {
                IsAutoRemoveDataCache = true,  // 启用删除数据缓存
                IsAutoDeleteQueryFilter = true, // 启用删除查询过滤器
                IsAutoUpdateQueryFilter = true, // 启用更新查询过滤器
                SqlServerCodeFirstNvarchar = true // 采用Nvarchar
            };
        }

        /// <summary>
        /// 配置Aop
        /// </summary>
        /// <param name="db"></param>
        /// <param name="enableConsoleSql"></param>
        public static void SetDbAop(this SqlSugarScopeProvider db)
        {
            var config = db.CurrentConnectionConfig;

            // 设置超时时间
            db.Ado.CommandTimeOut = 30;

            // 打印SQL语句

            #region 控制台输出

#if DEBUG
            db.Aop.OnLogExecuting = (sql, pars) =>
            {
                var originColor = Console.ForegroundColor;
                if (sql.StartsWith("SELECT", StringComparison.OrdinalIgnoreCase))
                    Console.ForegroundColor = ConsoleColor.Green;
                if (sql.StartsWith("UPDATE", StringComparison.OrdinalIgnoreCase) || sql.StartsWith("INSERT", StringComparison.OrdinalIgnoreCase))
                    Console.ForegroundColor = ConsoleColor.Yellow;
                if (sql.StartsWith("DELETE", StringComparison.OrdinalIgnoreCase))
                    Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("【" + DateTime.Now + "——执行SQL】\r\n" + UtilMethods.GetSqlString(config.DbType, sql, pars) + "\r\n");
                Console.ForegroundColor = originColor;
                Console.WriteLine("SqlSugar", "Info", sql + "\r\n" + db.Utilities.SerializeObject(pars.ToDictionary(it => it.ParameterName, it => it.Value)));
            };
#endif
            db.Aop.OnError = ex =>
            {
#if DEBUG
                if (ex.Parametres == null) return;
                var originColor = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.DarkRed;
                var pars = db.Utilities.SerializeObject(((SugarParameter[])ex.Parametres).ToDictionary(it => it.ParameterName, it => it.Value));
                Console.WriteLine("【" + DateTime.Now + "——错误SQL】\r\n" + UtilMethods.GetSqlString(config.DbType, ex.Sql, (SugarParameter[])ex.Parametres) + "\r\n");
                Console.ForegroundColor = originColor;
                Console.WriteLine("SqlSugar", "Error", $"{ex.Message}{Environment.NewLine}{ex.Sql}{pars}{Environment.NewLine}");
#endif
#if !DEBUG
                AppLogs.Error(ex);
                if (ex.Parametres == null) return;
                var pars = db.Utilities.SerializeObject(((SugarParameter[])ex.Parametres).ToDictionary(it => it.ParameterName, it => it.Value));
                AppLogs.Error($"{ex.Message}{Environment.NewLine}{ex.Sql}{pars}{Environment.NewLine}");
#endif
            };
#if DEBUG
            db.Aop.OnLogExecuted = (sql, pars) =>
            {
                // 执行时间超过5秒
                if (db.Ado.SqlExecutionTime.TotalSeconds > 5)
                {
                    var fileName = db.Ado.SqlStackTrace.FirstFileName; // 文件名
                    var fileLine = db.Ado.SqlStackTrace.FirstLine; // 行号
                    var firstMethodName = db.Ado.SqlStackTrace.FirstMethodName; // 方法名
                    var log = $"【所在文件名】：{fileName}\r\n【代码行数】：{fileLine}\r\n【方法名】：{firstMethodName}\r\n" + $"【sql语句】：{UtilMethods.GetSqlString(config.DbType, sql, pars)}";
                    var originColor = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.WriteLine(log);
                    Console.ForegroundColor = originColor;
                }
            };
#endif

            #endregion 控制台输出

            // 数据审计
            db.Aop.DataExecuting = (oldValue, entityInfo) =>
            {
                var applicationContext = ContainerLocator.Container.Resolve<IApplicationContext>();
                IDeletedFilter? deletedFilter = null;
                if (entityInfo.EntityValue is IDeletedFilter deleted)
                {
                    deletedFilter = deleted;
                }
                if (entityInfo.OperationType == DataFilterType.InsertByObject)
                {
                    if (entityInfo.PropertyName == "CreationTime")
                        entityInfo.SetValue(DateTime.Now);
                    if (entityInfo.PropertyName == "CreatorId")
                        entityInfo.SetValue(applicationContext.CurrentUser?.Id);
                    //if (entityInfo.PropertyName == "SystemTitle")
                    //    entityInfo.SetValue(applicationContext.SystemInfo.Title);
                }
                if (entityInfo.OperationType == DataFilterType.UpdateByObject && deletedFilter is not null && !deletedFilter.IsDeleted)
                {
                    if (entityInfo.PropertyName == "ModificationTime")
                        entityInfo.SetValue(DateTime.Now);
                    if (entityInfo.PropertyName == "ModificationId")
                        entityInfo.SetValue(applicationContext.CurrentUser?.Id);
                }
                if (entityInfo.OperationType == DataFilterType.UpdateByObject && deletedFilter is not null && deletedFilter.IsDeleted)
                {
                    if (entityInfo.PropertyName == "DeletionTime" && deletedFilter.DeletionTime is null)
                        entityInfo.SetValue(DateTime.Now);
                    if (entityInfo.PropertyName == "DeleterId" && deletedFilter.DeleterId is null)
                        entityInfo.SetValue(applicationContext.CurrentUser?.Id);
                }
            };

            // 配置假删除过滤器
            db.QueryFilter.AddTableFilter<IDeletedFilter>(u => u.IsDeleted == false);
            // 根据角色类型进行过滤数据
            Func<ICreationFilter, bool> personFilter = (entity) =>
            {
                var applicationContext = ContainerLocator.Container.Resolve<IApplicationContext>();
                if (applicationContext != null && applicationContext.CurrentUser is not null)
                {
                    if (applicationContext.CurrentUser.UserType != AppUserType.Admin)
                    {
                        return entity.CreatorId == applicationContext.CurrentUser.Id;
                    }
                }
                return true;
            };
            db.QueryFilter.AddTableFilter<ICreationFilter>(e => personFilter(e));
        }

        /// <summary>
        /// 初始化数据库
        /// </summary>
        /// <param name="db"></param>
        /// <param name="config"></param>
        private static void InitMainDatabase(this SqlSugarScope db, DbConnectionConfig config)
        {
            SqlSugarScopeProvider dbProvider = db.GetConnectionScope(config.ConfigId);
            // 初始化/创建数据库
            if (config.DbSettings.EnableInitDb)
            {
                var dbFileExists = File.Exists(Path.Combine(FilePathConst._dbPath, config.DbFilePath));
                if (config.DbType != SqlSugar.DbType.Oracle)
                {
                    dbProvider.DbMaintenance.CreateDatabase();
                    // 初始化表结构
                    CreateTableByEntity(db, false, typeof(VersionInfo), typeof(AppUser),
                        typeof(SchemeSet), typeof(AssociatedData),
                        typeof(Train), typeof(TrainContent), typeof(TrainResult),
                        typeof(Scheme), typeof(SchemeAppType), typeof(SchemeContent),
                        typeof(SchemeSetTemplate), typeof(TrainTemplate));
                }
                if (!dbFileExists)//不存在才会初始化种子数据
                {
                    // 初始化种子数据
                    if (config.DbSettings.EnableInitSeed)
                    {
                        Assembly assembly = Assembly.GetExecutingAssembly();
                        var seedDataTypes = assembly.GetTypes().Where(u => !u.IsInterface &&
                        !u.IsAbstract && u.IsClass && u.GetInterfaces().
                        Any(i => i.HasImplementedRawGeneric(typeof(ISqlSugarEntitySeedData<>)))).ToList();

                        foreach (var seedType in seedDataTypes)
                        {
                            var entityType = seedType.GetInterfaces().First().GetGenericArguments().First();
                            if (config.ConfigId == SqlSugarConst.MainConfigId) // 默认库（有系统表特性）才给予种子数据插入
                            {
                                if (entityType.GetCustomAttribute<SysTableAttribute>() == null)
                                    continue;
                            }

                            var instance = Activator.CreateInstance(seedType);
                            var hasDataMethod = seedType.GetMethod("HasData");
                            if (hasDataMethod != null)
                            {
                                var seedData = ((IEnumerable)hasDataMethod.Invoke(instance, null))?.Cast<object>();
                                if (seedData == null) continue;

                                var entityInfo = dbProvider.EntityMaintenance.GetEntityInfo(entityType);
                                if (entityInfo.Columns.Any(u => u.IsPrimarykey))
                                {
                                    // 按主键进行批量增加和更新
                                    var storage = dbProvider.StorageableByObject(seedData.ToList()).ToStorage();
                                    storage.AsInsertable.ExecuteCommand();
                                    storage.AsUpdateable.ExecuteCommand();
                                }
                                else
                                {
                                    // 无主键则只进行插入
                                    if (!dbProvider.Queryable(entityInfo.DbTableName, entityInfo.DbTableName).Any())
                                        dbProvider.InsertableByObject(seedData.ToList()).ExecuteCommand();
                                }
                            }
                        }
                    }
                }
            }
        }

        private static bool HasImplementedRawGeneric(this Type type, Type generic)
        {
            // 检查接口类型
            var isTheRawGenericType = type.GetInterfaces().Any(IsTheRawGenericType);
            if (isTheRawGenericType) return true;

            // 检查类型
            while (type != null && type != typeof(object))
            {
                isTheRawGenericType = IsTheRawGenericType(type);
                if (isTheRawGenericType) return true;
                type = type.BaseType;
            }

            return false;

            // 判断逻辑
            bool IsTheRawGenericType(Type type) => generic == (type.IsGenericType ? type.GetGenericTypeDefinition() : type);
        }

        /// <summary>
        /// 初始化租户业务数据库，对应不同文件夹的项目
        /// </summary>
        /// <param name="iTenant"></param>
        /// <param name="config"></param>
        public static void InitTenantDatabase(this ITenant iTenant, DbConnectionConfig config)
        {
            config.SetDbConfig();

            iTenant.AddConnection(config);
            ISqlSugarClient db = iTenant.GetConnectionScope(config.ConfigId);
            VersionInfo? versionInfo = null;
            bool isAdd = true;
            if (File.Exists(config.DbFilePath))
            {
                versionInfo = db.Queryable<VersionInfo>().Single();
                isAdd = false;
            }
            if (versionInfo is null)
            {
                isAdd = true;
                versionInfo = new VersionInfo() { Id = 1, Version = "0.0.0" };
            }
            if (config.DbSettings.EnableInitDb && !File.Exists(config.DbFilePath))
            {
                db.DbMaintenance.CreateDatabase();
            }

            if (Version.Parse(versionInfo.Version) < Version.Parse(config.Version))
            {
                CreateTableByEntity(db, false, typeof(VersionInfo));
                versionInfo.Version = config.Version;
            }
            SeedVersionInfo(db, versionInfo!, isAdd);
        }

        /// <summary>
        /// 根据实体类生成数据库表
        /// </summary>
        /// <param name="blnBackupTable">是否备份表</param>
        /// <param name="lstEntitys">指定的实体</param>
        private static void CreateTableByEntity(ISqlSugarClient sqlSugarClient, bool blnBackupTable, params Type[] lstEntitys)
        {
            if (lstEntitys.Length > 0)
            {
                if (sqlSugarClient.CodeFirst.GetDifferenceTables(lstEntitys).ToDiffList().Count > 0)
                {
                    if (blnBackupTable)
                    {
                        // BackupTable 生成表，并且修改类会备份 例如表A，备份表名为 A+时间
                        sqlSugarClient.CodeFirst.BackupTable().InitTables(lstEntitys);
                        // change entity backupTable
                    }
                    else
                    {
                        // InitTables函数是将实体转换成数据库表
                        sqlSugarClient.CodeFirst.InitTables(lstEntitys);
                    }
                }
            }
        }

        private static void SeedVersionInfo(ISqlSugarClient sqlSugarClient, VersionInfo version, bool isAdd = true)
        {
            if (isAdd)
            {
                sqlSugarClient.Insertable(version).ExecuteCommand();
            }
            else
            {
                sqlSugarClient.Updateable(version).ExecuteCommand();
            }
        }
    }
}