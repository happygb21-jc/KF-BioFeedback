using System.Globalization;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Kingfar.BioFeedback.Entities.Entities;

[SugarTable("bf_AppUsers")]
[Serializable]
[SysTable]
public class AppUser : FullEntity<Guid>, ICurrentUser, ISystemTitle
{
    public AppUser()
    { }

    public AppUser(string name, string userName, string? password)
    {
        Name = name;
        UserName = userName;
        Password = password;
        if (this.UserType == AppUserType.Subject && this.Birthday is null)
        {
            Birthday = DateTime.Parse($"{userName.Substring(6, 4)}-{userName.Substring(10, 2)}-{userName.Substring(12, 2)}");
        }
    }

    public AppUser(string name, string userName, string? password, string? phone, bool? isMale, DateTime? birthday, string? description, AppUserType userType = AppUserType.Subject)
    {
        Name = name;
        UserName = userName;
        if (userType != AppUserType.Subject && !string.IsNullOrEmpty(password))
            Password = password.GetMd5Hash();
        else
        {
            Password = null;
            Birthday = DateTime.Parse(userName.Substring(6, 4) + userName.Substring(10, 2) + userName.Substring(12, 2));
        }
        Phone = phone;
        IsMale = isMale;
        Birthday = birthday;
        Description = description;
        UserType = userType;
    }

    public void SetData(string name, string userName, string? phone, bool? isMale)
    {
        Name = name;
        UserName = userName;
        Phone = phone;
        IsMale = isMale;
        if (this.UserType==AppUserType.Subject && this.Birthday is null )
        {
            Birthday = DateTime.Parse($"{userName.Substring(6, 4)}-{userName.Substring(10, 2)}-{userName.Substring(12, 2)}");
        }
    }

    [SugarColumn(ColumnDescription = "姓名")]
    public string Name { get; set; } = default!;

    [SugarColumn(ColumnDescription = "账号")]
    public string UserName { get; set; } = default!;

    [SugarColumn(ColumnDescription = "密码")]
    public string? Password { get; set; } = default!;

    [SugarColumn(ColumnDescription = "电话")]
    public string? Phone { get; set; } = default!;

    [SugarColumn(ColumnDescription = "性别")]
    public bool? IsMale { get; set; } = default!;

    [SugarColumn(ColumnDescription = "生日")]
    public DateTime? Birthday { get; set; } = default!;


    [SugarColumn(ColumnDescription = "用户描述")]
    public string? Description { get; set; } = default!;

    [SugarColumn(ColumnDescription = "用户类型")]
    public AppUserType UserType { get; set; }

    [SugarColumn(ColumnDescription = "系统名称")]
    public string? SystemTitle { get; set; }

    [SugarColumn(IsIgnore = true)]
    public AppUser? Creator { get; set; }
}