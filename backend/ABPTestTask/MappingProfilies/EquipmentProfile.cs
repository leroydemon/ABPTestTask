using ABPTestTask.Common.Equipment;
using ABPTestTask.Common.Equipments;
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