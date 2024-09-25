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
            if (entityDto == null)
            {
                throw new ArgumentNullException(nameof(entityDto)); // Ensure entityDto is not null
            }

            // Fetch the hall information based on HallId
            var hall = await _hallRepository.GetByIdAsync(entityDto.HallId);
            if (hall == null)
            {
                throw new InvalidOperationException("Hall not found."); // Throw an error if the hall is not found
            }

            decimal totalPrice = 0;
            DateTime startTime = entityDto.StartDateTime;
            DateTime endTime = entityDto.EndDateTime;

            // Calculate price based on hourly rates and time slots
            while (startTime.TimeOfDay.Hours < endTime.TimeOfDay.Hours)
            {
                decimal hourlyRate = hall.Price;

                // Adjust hourly rate based on the time of day
                if (startTime.TimeOfDay >= TimeSpan.FromHours(18) && startTime.TimeOfDay < TimeSpan.FromHours(23))
                {
                    hourlyRate *= 0.8m; // Evening discount
                }
                else if (startTime.TimeOfDay >= TimeSpan.FromHours(6) && startTime.TimeOfDay < TimeSpan.FromHours(9))
                {
                    hourlyRate *= 0.9m; // Morning discount
                }
                else if (startTime.TimeOfDay >= TimeSpan.FromHours(12) && startTime.TimeOfDay < TimeSpan.FromHours(14))
                {
                    hourlyRate *= 1.15m; // Peak hours surcharge
                }

                totalPrice += hourlyRate; // Add hourly rate to total price
                startTime = startTime.Add(TimeSpan.FromHours(1)); // Move to the next hour
            }

            // Add services price if any services are selected
            if (entityDto.Services?.Any() == true)
            {
                totalPrice += entityDto.Services.Sum(service => service.Price);
            }

            return totalPrice; // Return the total calculated price
        }

        public async Task<decimal> BookingHall(BookingDto entityDto)
        {
            if (entityDto == null)
            {
                throw new ArgumentNullException(nameof(entityDto)); // Ensure entityDto is not null
            }

            var entity = _mapper.Map<Booking>(entityDto); // Map DTO to Booking entity

            entity.EquipmentsListJson = JsonSerializer.Serialize(entityDto.Services); // Serialize services to JSON

            await _bookingRepository.AddAsync(entity); // Add booking entity to the repository

            decimal totalPrice = await CalculatePrice(entityDto); // Calculate and return the price
            return totalPrice; // Return total price
        }

        public async Task<IEnumerable<BookingDto>> SearchAsync(BookingFilter filter)
        {
            var spec = new BookingSpecification(filter); // Create a new specification for filtering
            var bookings = await _bookingRepository.ListAsync(spec); // Get bookings based on the specification

            // Deserialize equipment list from JSON for each booking
            foreach (var booking in bookings)
            {
                if (!string.IsNullOrEmpty(booking.EquipmentsListJson))
                {
                    booking.Equipments = JsonSerializer.Deserialize<List<Equipment>>(booking.EquipmentsListJson); // Deserialize the JSON string
                }
            }

            return _mapper.Map<List<BookingDto>>(bookings); // Map entities to BookingDto and return
        }

        public async Task RemoveAsync(Guid id)
        {
            // Retrieve the booking entity by ID
            var entity = await _bookingRepository.GetByIdAsync(id);

            // Check if the entity exists
            if (entity == null)
            {
                throw new InvalidOperationException("Booking not found."); // Throw an exception if the booking does not exist
            }

            // Delete the booking entity
            await _bookingRepository.DeleteAsync(entity);
        }

        public async Task<BookingDto> GetByIdAsync(Guid id)
        {
            // Retrieve the booking entity by ID
            var entity = await _bookingRepository.GetByIdAsync(id);

            // Check if the booking entity exists
            if (entity == null)
            {
                throw new InvalidOperationException("Booking not found."); // Throw an exception if the booking does not exist
            }

            // Deserialize the JSON string to the Equipment list if it exists
            if (!string.IsNullOrEmpty(entity.EquipmentsListJson))
            {
                entity.Equipments = JsonSerializer.Deserialize<List<Equipment>>(entity.EquipmentsListJson);
            }

            // Map the booking entity to BookingDto and return it
            return _mapper.Map<BookingDto>(entity);
        }

        public async Task<BookingDto> UpdateAsync(BookingDto entity)
        {
            // Check if the entity is null
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity)); // Throw an exception if the entity is null
            }

            // Map the BookingDto to Booking entity
            var bookingEntity = _mapper.Map<Booking>(entity);

            // Serialize the services list to JSON for storage
            bookingEntity.EquipmentsListJson = JsonSerializer.Serialize(entity.Services);

            // Update the booking entity in the repository and get the updated entity
            var updatedEntity = await _bookingRepository.UpdateAsync(bookingEntity);

            // Map the updated Booking entity back to BookingDto and return
            return _mapper.Map<BookingDto>(updatedEntity);
        }
    }
}
