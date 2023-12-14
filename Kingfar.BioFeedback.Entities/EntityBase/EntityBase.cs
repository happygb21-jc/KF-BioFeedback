using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Kingfar.BioFeedback.Entites
{
    public class Entity<T>
    {
        /// <summary>
        /// ID，通常为主键
        /// </summary>
        [SugarColumn(IsNullable = false, IsPrimaryKey = true, ColumnDescription = "主键")]
        public T Id { get; set; } = default!;
    }

    public class CreationEntity<T> : Entity<T>
    {
        [SugarColumn(ColumnDescription = "创建时间")]
        public DateTime? CreationTime { get; set; }

        [SugarColumn(ColumnDescription = "创建人")]
        public Guid? CreatorId { get; set; }
    }

    public class ModificationEntity<T> : CreationEntity<T>
    {
        [SugarColumn(ColumnDescription = "更新时间", IsOnlyIgnoreInsert = true)]
        public virtual DateTime? ModificationTime { get; set; }

        [SugarColumn(ColumnDescription = "修改人", IsOnlyIgnoreInsert = true)]
        public virtual Guid? ModificationId { get; set; }
    }

    public class FullEntity<T> : ModificationEntity<T>, IDeletedFilter
    {
        [SugarColumn(ColumnDescription = "软删除")]
        public virtual bool IsDeleted { get; set; } = false;

        [SugarColumn(ColumnDescription = "删除时间", IsOnlyIgnoreInsert = true)]
        public virtual DateTime? DeletionTime { get; set; }

        [SugarColumn(ColumnDescription = "删除人", IsOnlyIgnoreInsert = true)]
        public virtual Guid? DeleterId { get; set; }
    }
}