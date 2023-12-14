using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kingfar.BioFeedback.Shared
{
    /// <summary>
    /// 数据库配置选项
    /// </summary>
    public sealed class DbConnectionOptions
    {
        /// <summary>
        /// 数据库集合
        /// </summary>
        public List<DbConnectionConfig> ConnectionConfigs { get; set; } = default!;

    }

    /// <summary>
    /// 数据库连接配置
    /// </summary>
    public sealed class DbConnectionConfig : ConnectionConfig
    {
        /// <summary>
        /// 数据库设置
        /// </summary>
        public DbSettings DbSettings { get; set; } = default!;

        public string DbFilePath { get; set; } = default!;

        /// <summary>
        /// 数据库版本
        /// </summary>
        public string Version { get; set; } = default!;
        
    }

    /// <summary>
    /// 数据库设置
    /// </summary>
    public sealed class DbSettings
    {
        /// <summary>
        /// 启用库表初始化
        /// </summary>
        public bool EnableInitDb { get; set; }
        /// <summary>
        /// 插入种子数据
        /// </summary>
        public bool EnableInitSeed { get; set; }

        /// <summary>
        /// 启用驼峰转下划线
        /// </summary>
        public bool EnableUnderLine { get; set; }
    }
}
