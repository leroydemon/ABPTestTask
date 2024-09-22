using AutoMapper;
using BussinesLogic.EntitiesDto;

public class BookingProfile : Profile
{
    public BookingProfile()
    {
        CreateMap<Booking, BookingDto>()
            .ReverseMap();
    }
}
