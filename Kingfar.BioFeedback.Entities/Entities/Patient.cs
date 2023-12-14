using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kingfar.BioFeedback.Entities.Entities;

[SugarTable("bf_Patients")]
[Serializable]
[SysTable]
public class Patient : FullEntity<Guid>, ICurrentUser
{
    [SugarColumn(ColumnDescription = "姓名")]
    public string Name { get; set; } = string.Empty;

    public AppUserType UserType => AppUserType.Subject;

    [SugarColumn(ColumnDescription = "身份证号")]
    public string IdCard { get; set; } = default!;

    [SugarColumn(ColumnDescription = "性别")]
    public bool? IsMale { get; set; } = default!;

    [SugarColumn(ColumnDescription = "年龄")]
    public int Age { get; set; }

    [SugarColumn(ColumnDescription = "电话")]
    public string? Phone { get; set; } = default!;

    [SugarColumn(ColumnDescription = "用户描述")]
    public string? Description { get; set; } = default!;

    [SqlSugar.SugarColumn(IsIgnore = true)]
    public AppUser? Creator { get; set; }
}