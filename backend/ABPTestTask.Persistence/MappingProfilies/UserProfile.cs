using ABPTestTask.Common.User;
using AutoMapper;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<User, UserEntity>()
            .ReverseMap();
    }
}