using ABPTestTask.Common.Bookings;
using AutoMapper;
using BussinesLogic.EntitiesDto;

public class BookingProfile : Profile
{
    public BookingProfile()
    {
        CreateMap<Booking, BookingDto>()
            .ReverseMap()
            .ForMember(dest => dest.Equipments, opt => opt.MapFrom(src => src.Services));
    }
}
