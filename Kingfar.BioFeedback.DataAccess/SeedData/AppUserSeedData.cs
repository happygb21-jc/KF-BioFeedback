using Kingfar.BioFeedback.Entities.Entities;
using Kingfar.BioFeedback.Shared;

namespace Kingfar.BioFeedback.DataAccess.SeedData
{
    internal class AppUserSeedData : ISqlSugarEntitySeedData<AppUser>
    {
        public IEnumerable<AppUser> HasData()
        {
            return new[] {
                new AppUser(){Name="管理员",UserName="admin",Password="123123".GetMd5Hash(),UserType=AppUserType.Admin,Description="管理员账号"}
            };
        }
    }

    //资源种子数据，音频，视频等

    //训练内容种子数据
}