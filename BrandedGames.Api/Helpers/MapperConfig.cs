using AutoMapper;
using BrandedGames.Common.Models;
using BrandedGames.Entities;

public class MapperConfig : Profile
{
    public MapperConfig()
    {
        Guid? userId = null;

        CreateMap<PlatformType, PlatformTypeModel>();
        CreateMap<GameType, GameTypeModel>();
        CreateMap<GameFeature, FeatureModel>();
    }
}
