
namespace Kingfar.BioFeedback.Shared
{
    public interface IAppMapper
    {
        AutoMapper.IMapper Current { get; }

        TDestination Map<TDestination>(object source);
    }
}
