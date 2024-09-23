using AutoMapper;
using BussinesLogic.EntitiesDto;
using BussinesLogic.Interfaces;
using DbLevel.Interfaces;
using Domain.Entities;
using Domain.Filters;
using Domain.Specifications;
using System.Text.Json;

namespace BussinesLogic.Services
{
    public class BookingService : IBookingService
    {
        private readonly IRepository<Booking> _bookingRepository;
        private readonly IMapper _mapper;
        private readonly IRepository<Hall> _hallRepository;
        public BookingService(IRepository<Booking> bookingRepository, IMapper mapper, IRepository<Hall> hallRepository)
        {
            _bookingRepository = bookingRepository;
            _mapper = mapper;
            _hallRepository = hallRepository;
        }

        public async Task<decimal> CalculatePrice(BookingDto entityDto)
        {
            if (entityDto.Services == null || !entityDto.Services.Any())
            {
                throw new ArgumentException("Services cannot be null or empty.", nameof(entityDto.Services));
            }

            var hall = await _hallRepository.GetByIdAsync(entityDto.HallId);
            if (hall == null)
            {
                throw new InvalidOperationException("Hall not found.");
            }

            decimal totalPrice = 0;
            DateTime startTime = entityDto.StartDateTime;
            DateTime endTime = entityDto.EndDateTime;

            while (startTime < endTime)
            {
                decimal hourlyRate = hall.Price;

                if (startTime.TimeOfDay >= TimeSpan.FromHours(9) && startTime.TimeOfDay < TimeSpan.FromHours(18))
                {
                }
                else if (startTime.TimeOfDay >= TimeSpan.FromHours(18) && startTime.TimeOfDay < TimeSpan.FromHours(23))
                {
                    hourlyRate *= 0.8m; 
                }
                else if (startTime.TimeOfDay >= TimeSpan.FromHours(6) && startTime.TimeOfDay < TimeSpan.FromHours(9))
                {
                    hourlyRate *= 0.9m; 
                }
                else if (startTime.TimeOfDay >= TimeSpan.FromHours(12) && startTime.TimeOfDay < TimeSpan.FromHours(14))
                {
                    hourlyRate *= 1.15m; 
                }

                totalPrice += hourlyRate;

                startTime = startTime.Add(TimeSpan.FromHours(1));
            }

            totalPrice += entityDto.Services.Sum(service => service.Price);

            return totalPrice;
        }

        public async Task<decimal> BookingHall(BookingDto entityDto)
        {
            if (entityDto == null)
            {
                throw new ArgumentNullException(nameof(entityDto));
            }

            var entity = _mapper.Map<Booking>(entityDto);

            entity.EquipmentsListJson = JsonSerializer.Serialize(entityDto.Services);
            await _bookingRepository.AddAsync(entity);

            return await CalculatePrice(entityDto);
        }

        public async Task<IEnumerable<BookingDto>> SearchAsync(BookingFilter filter)
        {
            var spec = new BookingSpecification(filter);
            var bookings = await _bookingRepository.ListAsync(spec);

            foreach (var booking in bookings)
            {
                if (!string.IsNullOrEmpty(booking.EquipmentsListJson))
                {
                    booking.Equipments = JsonSerializer.Deserialize<List<Equipment>>(booking.EquipmentsListJson);
                }
            }

            return _mapper.Map<List<BookingDto>>(bookings);
        }

        public async Task RemoveAsync(Guid Id)
        {
            var entity = await _bookingRepository.GetByIdAsync(Id);
            await _bookingRepository.DeleteAsync(entity);
        }

        public async Task<BookingDto> GetByIdAsync(Guid Id)
        {
            var entity = await _bookingRepository.GetByIdAsync(Id);

            if (!string.IsNullOrEmpty(entity.EquipmentsListJson))
            {
                entity.Equipments = JsonSerializer.Deserialize<List<Equipment>>(entity.EquipmentsListJson);
            }

            return _mapper.Map<BookingDto>(entity);
        }

        public async Task<BookingDto> UpdateAsync(BookingDto entity)
        {
            var bookingEntity = _mapper.Map<Booking>(entity);
            bookingEntity.EquipmentsListJson = JsonSerializer.Serialize(entity.Services);

            var updatedEntity = await _bookingRepository.UpdateAsync(bookingEntity);

            return _mapper.Map<BookingDto>(updatedEntity);
        }
    }
}
