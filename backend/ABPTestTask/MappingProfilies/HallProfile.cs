using ABPTestTask.Common.Hall;
using AutoMapper;
using BussinesLogic.EntitiesDto;

public class HallProfile : Profile
{
    public HallProfile()
    {
        CreateMap<Hall, HallDto>()
            .ReverseMap();
    }
}