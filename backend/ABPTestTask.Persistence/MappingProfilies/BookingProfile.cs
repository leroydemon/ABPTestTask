using AutoMapper;

public class BookingProfile : Profile
{
    public BookingProfile()
    {
        CreateMap<Booking, BookingEntity>()
            .ReverseMap();
    }
}
