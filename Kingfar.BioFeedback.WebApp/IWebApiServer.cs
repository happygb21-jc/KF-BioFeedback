using Prism.Ioc;

namespace Kingfar.BioFeedback.WebHost
{
    public interface IWebApiServer
    {
        void Start(string apiUrl, IContainerProvider containerProvider);

        void Stop();
    }
}