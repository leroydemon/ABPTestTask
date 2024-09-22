using AutoMapper;
using BussinesLogic.EntitiesDto;
using BussinesLogic.Interfaces;
using DbLevel.Interfaces;
using Domain.Entities;
using Domain.Filters;
using Domain.Specifications;

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
            decimal totalPrice = 0;
            if (entityDto.Services == null || !entityDto.Services.Any())
            {
                throw new Exception();
            }

            TimeSpan startTime = entityDto.StartDateTime.TimeOfDay;
            TimeSpan endTime = entityDto.EndDateTime.TimeOfDay;

            var hall = await _hallRepository.GetByIdAsync(entityDto.HallId);
            if (hall == null)
            {
                throw new InvalidOperationException("Hall not found.");
            }

            while (startTime < endTime)
            {
                // Стандартные часы (09:00 - 18:00)
                if (startTime >= TimeSpan.FromHours(9) && startTime < TimeSpan.FromHours(18))
                {
                    totalPrice += hall.Price;
                }
                // Вечерние часы (18:00 - 23:00) - скидка 20%
                else if (startTime >= TimeSpan.FromHours(18) && startTime < TimeSpan.FromHours(23))
                {
                    totalPrice += hall.Price * 0.8m; // 20% скидка
                }
                // Утренние часы (06:00 - 09:00) - скидка 10%
                else if (startTime >= TimeSpan.FromHours(6) && startTime < TimeSpan.FromHours(9))
                {
                    totalPrice += hall.Price * 0.9m; // 10% скидка
                }
                // Пиковые часы (12:00 - 14:00) - наценка 15%
                else if (startTime >= TimeSpan.FromHours(12) && startTime < TimeSpan.FromHours(14))
                {
                    totalPrice += hall.Price * 1.15m; // 15% наценка
                }
                else
                {
                    totalPrice += hall.Price;
                }

                // Переход на следующий час
                startTime = startTime.Add(TimeSpan.FromHours(1));
            }

            entityDto.Services.ForEach(service => totalPrice += service.Price);

            return totalPrice;
        }

        public async Task<decimal> BookingHall(BookingDto entityDto)
        {
            if (entityDto == null)
            {
                throw new ArgumentNullException(nameof(entityDto));
            }

            var entity = _mapper.Map<Booking>(entityDto);
            var total = await CalculatePrice(entityDto);
            await _bookingRepository.AddAsync(entity);

            return total;
        }

        public async Task<IEnumerable<BookingDto>> SearchAsync(BookingFilter filter)
        {
            var spec = new BookingSpecification(filter);
            var bookings = await _bookingRepository.ListAsync(spec);

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
            return _mapper.Map<BookingDto>(entity);
        }

        public async Task<BookingDto> UpdateAsync(BookingDto entity)
        {
            var updatedEntity = await _bookingRepository.UpdateAsync(_mapper.Map<Booking>(entity));

            return _mapper.Map<BookingDto>(updatedEntity);
        }
    }
}
