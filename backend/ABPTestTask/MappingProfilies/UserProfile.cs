using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ABPTestTask.Common.User;
using AutoMapper;
using BussinesLogic.EntitiesDto;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<User, UserDto>()
            .ReverseMap();
    }
}