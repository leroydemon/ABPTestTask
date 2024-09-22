using AutoMapper;
using BussinesLogic.EntitiesDto;
using Domain.Entities;

public class HallProfile : Profile
{
    public HallProfile()
    {
        CreateMap<Hall, HallDto>()
            .ReverseMap();
    }
}