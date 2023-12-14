using Kingfar.BioFeedback.Shared;
using Prism.Ioc;

namespace Kingfar.BioFeedback.Services
{
    public class ServiceBase
    {
        public readonly IApplicationContext ApplicationContext= ContainerLocator.Container.Resolve<IApplicationContext>();
        public readonly IAppMapper AppMapper = ContainerLocator.Container.Resolve<IAppMapper>();
    }
}
