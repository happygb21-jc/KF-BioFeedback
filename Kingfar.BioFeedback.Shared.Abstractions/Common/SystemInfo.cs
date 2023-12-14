using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kingfar.BioFeedback.Shared
{
    public class SystemInfo
    {
        public string Title { get; set; } = "BioNeuro";
        public string Description { get; set; } = string.Empty;
        public string Version { get; set; } = "0.0.1";
        public SystemUserVersion SystemUserVersion { get; set; } = SystemUserVersion.Multi;
    }

    public enum SystemUserVersion
    {
        Single,
        Multi
    }
}