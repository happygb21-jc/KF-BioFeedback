using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kingfar.BioFeedback.Mvvm.Dto
{
    public class SchemeSetDto
    {
        public SchemeSetDto(string name, SchemeSetType type, List<UserDto> users, List<SchemeDto> schemes, string creator, DateTime creationTime, double proportion)
        {
            Name = name;
            Type = type;
            Users = users;
            Schemes = schemes;
            Creator = creator;
            CreationTime = creationTime;
            Proportion = proportion;
        }

        public string Name { get; set; }
        public SchemeSetType Type { get; set; }
        public List<UserDto> Users { get; set; }
        public List<SchemeDto> Schemes { get; set; }
        public string Creator { get; set; }
        public DateTime CreationTime { get; set; }
        public double Proportion { get; set; }
    }

    public enum SchemeSetType {
        [Description("个人")]
        Single,
        [Description("团队")]
        Multiple
    }
}
