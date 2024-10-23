using ABPTestTask.Common.Hall;
using AutoMapper;

public class HallProfile : Profile
{
    public HallProfile()
    {
        CreateMap<Hall, HallEntity>()
            .ReverseMap();
    }
}