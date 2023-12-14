using Kingfar.BioFeedback.Entites;
using Kingfar.BioFeedback.Shared;

namespace Kingfar.BioFeedback.Services
{
    public class AppUserDto
    {
        public AppUserDto()
        { }

        public AppUserDto(string name, string userName, string? password, string? phone, bool? isMale, DateTime? birthday, string? description, AppUserType userType)
        {
            Name = name;
            UserName = userName;
            Password = password;
            Phone = phone;
            IsMale = isMale;
            Birthday = birthday;
            Description = description;
            UserType = userType;
        }

        public void SetValues(string name, string userName, string? password, string? phone, bool? isMale, DateTime? birthday, string? description)
        {
            Name = name;
            UserName = userName;
            Password = password;
            Phone = phone;
            IsMale = isMale;
            Birthday = birthday;
            Description = description;
            UserType = AppUserType.Main;
        }

        public void SetValues(string name, string userName, string? phone, bool? isMale, string? description)
        {
            Name = name;
            UserName = userName;
            Phone = phone;
            IsMale = isMale;
            Description = description;
            UserType = AppUserType.Subject;
        }

        public Guid? Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string? Password { get; set; }
        public string? Phone { get; set; }
        public bool? IsMale { get; set; }
        public DateTime? Birthday { get; set; }
        public string? Description { get; set; }

        public DateTime? CreationTime { get; set; }

        public AppUserType UserType { get; set; }

        public string Creator { get; set; }
    }
}