using AutoMapper.Configuration.Annotations;
using NewLife;
using NewLife.Configuration;
using System.ComponentModel;

namespace Kingfar.BioFeedback.Shared.Abstractions.Common;

/// <summary>系统设置。提供系统名称、版本等基本设置</summary>
[DisplayName("系统设置")]
[Config("SysSetting", "json")]
public class SysSetting : Config<SysSetting>
{
    public const string SystemName = "BioFeedback";

    #region 属性

    /// <summary>系统名称</summary>
    [DisplayName("医院名称")]
    [Description("用于标识所使用医院中文名")]
    public string HospitalName { get; set; } = string.Empty;

    /// <summary>系统名称</summary>
    [DisplayName("系统名称")]
    [Description("用于标识系统的中文名")]
    public string SoftwareName { get; set; } = string.Empty;

    /// <summary>系统版本</summary>
    [DisplayName("系统版本")]
    public string Version { get; set; } = string.Empty;

    /// <summary>系统英文名称</summary>
    [DisplayName("显示名称")]
    [Description("用于标识系统的英文名")]
    public string EnglishName { get; set; } = string.Empty;

    /// <summary>公司</summary>
    [DisplayName("公司")]
    public string Company { get; } = "北京津发科技股份有限公司";

    /// <summary>电话</summary>
    [DisplayName("电话")]
    public string Telephone { get; } = "400-811-3950";

    /// <summary>数据存储路径</summary>
    [DisplayName("数据存储路径")]
    public string DataSavePath { get; set; } = AppDomain.CurrentDomain.BaseDirectory;

    /// <summary>数据库存储路径变更需要直接迁移数据库文件</summary>
    /// 如何避免数据库占用时迁移呢？复制数据库，保留原始？
    [DisplayName("数据库存储路径")]
    public string DBSavePath { get; set; } = AppDomain.CurrentDomain.BaseDirectory;

    [DisplayName("服务IP")]
    public string HostIP { get; set; } = "192.168.1.241";

    [DisplayName("服务端口")]
    public string HostPort { get; set; } = "9527";

    [DisplayName("自动开启接口服务")]
    public bool IsAutoStartServer { get; set; }

    #endregion 属性

    #region 方法

    /// <summary>加载后触发</summary>

    protected override void OnLoaded()
    {
        if (IsNew)
        {
            //Todo 后续是否要增加软件存储路径更换的方式，要在SysSetting中追加配置
            //var documentPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            HospitalName = "xx医院";
            SoftwareName = "多参数生物反馈系统";
            Version = "0.0.1";
            EnglishName = "Multiparametric BioNeuro Feedback System";
            DataSavePath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            DBSavePath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        }

        base.OnLoaded();
    }

    #endregion 方法
}