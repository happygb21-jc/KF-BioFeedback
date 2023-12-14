using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kingfar.BioFeedback.Mvvm.Dto
{
    public class UserDto
    {
        public UserDto() { Id = Guid.NewGuid(); }

        public UserDto(string name, string userName)
        {
            Name = name;
            UserName = userName;
        }

        public UserDto(Guid id, string name, string userName, string password, bool isMale, DateTime birthday, string phone, DateTime creationTime, string description)
        {
            Id = id;
            Name = name;
            UserName = userName;
            Password = password;
            IsMale = isMale;
            Birthday = birthday;
            Phone = phone;
            CreationTime = creationTime;
            Description = description;
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool IsMale { get; set; }
        public DateTime Birthday { get; set; } = DateTime.Now;
        public string Phone { get; set; }
        public DateTime CreationTime { get; set; }
        public string Description { get; set; }

    }
}
