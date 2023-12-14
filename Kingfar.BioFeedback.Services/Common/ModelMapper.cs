using AutoMapper;
using System.Text.Json;

namespace Kingfar.BioFeedback.Services
{
    public class ModelMapper : Profile
    {
        public ModelMapper()
        {
            JsonSerializerOptions options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            CreateMap<AppUser, AppUserDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
                .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.Phone))
                .ForMember(dest => dest.IsMale, opt => opt.MapFrom(src => src.IsMale))
                .ForMember(dest => dest.Birthday, opt => opt.MapFrom(src => src.Birthday))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.UserType, opt => opt.MapFrom(src => src.UserType))
                .ForMember(dest => dest.CreationTime, opt => opt.MapFrom(src => src.CreationTime))
                .ForMember(dest => dest.Creator, opt => opt.MapFrom(src => src.Creator != null ? src.Creator.Name : string.Empty))
                .Ignore(dest => dest.Password);

            CreateMap<SchemeSet, SchemeSetDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                //.ForMember(dest => dest.TrainNames, opt => opt.MapFrom(src => src.Trains.Select(e => e.SoftwareName)))//后续完善
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type))
                .ForMember(dest => dest.IsPublished, opt => opt.MapFrom(src => src.IsPublished))
                .ForMember(dest => dest.CreationTime, opt => opt.MapFrom(src => src.CreationTime));

            CreateMap<Scheme, SchemeDto>()
               .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
               .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
               .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type))
               .ForMember(dest => dest.ApplicationTypes, opt => opt.MapFrom(src => src.ApplicationTypes.Select(e => e.Type)))
               .ForMember(dest => dest.CreationTime, opt => opt.MapFrom(src => src.CreationTime));

            CreateMap<Scheme, AddOrUpdateSchemeDto>()
              .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
              .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
              .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type))
              .ForMember(dest => dest.ApplicationTypes, opt => opt.MapFrom(src => src.ApplicationTypes.Select(e => e.Type)));

            CreateMap<SchemeSetTemplate, SchemeSetTemplateDto>()
               .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
               .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
               //.ForMember(dest => dest.TrainNames, opt => opt.MapFrom(src => src.Trains.Select(e => e.SoftwareName)))//后续完善
               .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type))
               .ForMember(dest => dest.CreationTime, opt => opt.MapFrom(src => src.CreationTime));

            CreateMap<Experiment, ExperimentDto>()
               .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
               .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
               .ForMember(dest => dest.ModelType, opt => opt.MapFrom(src => src.ModelType))
               .ForMember(dest => dest.Cover, opt => opt.MapFrom(src => src.Cover));
            CreateMap<Train, TrainDetialDto>()
               .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
               .ForMember(dest => dest.Experiments, opt => opt.MapFrom(src => src.Content.Experiments))
               .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));
            CreateMap<SchemeSet, SchemeSetDetailDto>();
        }
    }
}