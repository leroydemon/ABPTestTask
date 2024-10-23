using ABPTestTask.Common.Equipment;
using AutoMapper;
using BussinesLogic.EntitiesDto;

public class EquipmentProfile : Profile
{
    public EquipmentProfile()
    {
        CreateMap<Equipment, EquipmentDto>()
            .ReverseMap();
    }
}