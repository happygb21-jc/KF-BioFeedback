using AutoMapper;
using System;

namespace Kingfar.BioFeedback.Shared
{
    public class AppMapper : IAppMapper
    {
        public IMapper Current { get; }

        public TDestination Map<TDestination>(object source) => Current.Map<TDestination>(source);

        public AppMapper()
        {
            var configuration = new MapperConfiguration(configure =>
            {
                var assemblys = AppDomain.CurrentDomain.GetAssemblies();
                configure.AddMaps(assemblys);
            });
            Current = configuration.CreateMapper();
        }
    }
}