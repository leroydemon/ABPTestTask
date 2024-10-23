using ABPTestTask.Common.Equipment;
using ABPTestTask.Common.Equipments;
using AutoMapper;

public class EquipmentProfile : Profile
{
    public EquipmentProfile()
    {
        CreateMap<Equipment, EquipmentEntity>()
            .ReverseMap();
    }
}