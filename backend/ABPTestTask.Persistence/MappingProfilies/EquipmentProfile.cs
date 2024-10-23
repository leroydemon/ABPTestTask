﻿using ABPTestTask.Common.Equipment;
using AutoMapper;

public class EquipmentProfile : Profile
{
    public EquipmentProfile()
    {
        CreateMap<Equipment, EquipmentEntity>()
            .ReverseMap();
    }
}