using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kingfar.BioFeedback.Entites
{
    public interface IDeletedFilter
    {
        /// <summary>
        /// 软删除
        /// </summary>
        bool IsDeleted { get; set; }
        Guid? DeleterId { get; set; }
        DateTime? DeletionTime { get; set; }
    }
}
