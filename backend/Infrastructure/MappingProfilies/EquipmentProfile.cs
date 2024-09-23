using AutoMapper;
using BussinesLogic.EntitiesDto;
using Domain.Entities;

public class EquipmentProfile : Profile
{
    public EquipmentProfile()
    {
        CreateMap<Equipment, EquipmentDto>()
            .ReverseMap();
    }
}