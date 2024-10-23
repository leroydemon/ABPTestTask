using ABPTestTask.Common.Bookings;
using ABPTestTask.DAL.Entities;
using AutoMapper;

public class BookingProfile : Profile
{
    public BookingProfile()
    {
        CreateMap<Booking, BookingEntity>()
            .ReverseMap();
    }
}
