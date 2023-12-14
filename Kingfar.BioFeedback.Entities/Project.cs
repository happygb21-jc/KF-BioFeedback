using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kingfar.BioFeedback.DataAccess
{
    [SugarTable("sys_Projects", "项目表")] //表的别名处理，可以和实体类不一样
    [Serializable]
    [SysTable]
    public class Project : CreationEntity<Guid>
    {
        [SugarColumn(ColumnDescription = "名称", Length = 512)]
        public required string Name { get; set; }
        [SugarColumn(ColumnDescription = "文件夹路径")]
        public required string FolderPath { get; set; }
        [SugarColumn(ColumnDescription = "版本")]
        public required Version Version { get; set; }
    }
}
