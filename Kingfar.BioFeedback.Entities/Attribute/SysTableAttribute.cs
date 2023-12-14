namespace Kingfar.BioFeedback;

/// <summary>
/// 系统表特性 （用于指定SqlSugarClient 访问主库链接）
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
public class SysTableAttribute : Attribute
{
}