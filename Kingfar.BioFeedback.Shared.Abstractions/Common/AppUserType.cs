using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kingfar.BioFeedback.Shared
{
    public enum AppUserType
    {
        [Description("管理员")]
        Admin,

        [Description("医生")]
        Main,

        [Description("患者")]
        Subject
    }
}