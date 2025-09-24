using AutoMapper;
using VideoStreaming.Common.Models.Stream;
using VideoStreaming.Common.Models.User;
using VideoStreaming.Entities;

public class MapperConfig : Profile
{
    public MapperConfig()
    {
        Guid? userId = null;

        CreateMap<User, UserInitialsModel>();

        CreateMap<User, UserMeModel>()
            .IncludeBase<User, UserInitialsModel>();

        CreateMap<Meet, StreamModel>()
            .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.StreamType));
    }
}
