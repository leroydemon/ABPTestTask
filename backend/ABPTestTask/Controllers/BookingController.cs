using ABPTestTask.Common.Booking;
using BussinesLogic.EntitiesDto;
using Domain.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace ABPTestTask.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly IBookingService _bookingService;
        private readonly ILogger<BookingController> _logger;
        private readonly IMapper<> _mapper;

        public BookingController(IBookingService bookingService, ILogger<BookingController> logger)
        {
            _bookingService = bookingService;
            _logger = logger;
        }

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<BookingDto>>> SearchAsync([FromQuery] BookingFilter filter)
        {
            // Check if the filter is null
            if (filter == null)
            {
                return BadRequest(new { Message = "Filter cannot be null." }); // Return a bad request if the filter is null
            }

            // Call the search method from the service to get the list of bookings
            var entities = await _bookingService.SearchAsync(filter);

            // Check if no entities were found
            if (entities == null || !entities.Any())
            {
                return NotFound(new { Message = "No bookings found." }); // Return not found if no bookings match the filter
            }

            return Ok(entities); // Return the list of bookings
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveAsync(Guid id)
        {
            try
            {
                // Call the service to remove the hall
                await _bookingService.RemoveAsync(id);

                // Return a confirmation of deletion
                return Ok(new { Message = "Conference hall deleted successfully." });
            }
            catch (InvalidOperationException ex)
            {
                // Handle specific errors related to deletion
                return NotFound(new { Message = ex.Message }); // Return 404 if the hall was not found
            }
            catch (Exception ex)
            {
                // Handle unexpected errors
                _logger.LogError(ex, "An error occurred while deleting the conference hall.");
                return StatusCode(StatusCodes.Status500InternalServerError, new { Message = "An error occurred while processing your request." });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(Guid id)
        {
            try
            {
                // Call the service to retrieve the booking by ID
                var entity = await _bookingService.GetByIdAsync(id);

                // Return the retrieved booking
                return Ok(entity);
            }
            catch (InvalidOperationException ex)
            {
                // Handle the case where the booking was not found
                return NotFound(new { Message = ex.Message }); // Return 404 if the booking was not found
            }
            catch (Exception ex)
            {
                // Handle unexpected errors
                _logger.LogError(ex, "An error occurred while retrieving the booking.");
                return StatusCode(StatusCodes.Status500InternalServerError, new { Message = "An error occurred while processing your request." });
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateAsync(BookingDto entityDto)
        {
            // Check if the provided booking DTO is null
            if (entityDto == null)
            {
                return BadRequest(new { Message = "Invalid booking data." }); // Return a 400 Bad Request if the DTO is null
            }

            // Update the booking entity using the service
            try
            {
                await _bookingService.UpdateAsync(_mapper.Map<Booking>(entityDto));
                return Ok(new { Message = "Booking updated successfully." }); // Return 200 OK on successful update
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { Message = ex.Message }); // Return 404 if the booking was not found
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating the booking.");
                return StatusCode(StatusCodes.Status500InternalServerError, new { Message = "An error occurred while processing your request." }); // Handle unexpected errors
            }
        }

        [HttpPost("calculate")]
        public async Task<IActionResult> CalculatePrice(BookingDto entityDto)
        {
            // Check if the provided booking DTO is null
            if (entityDto == null)
            {
                return BadRequest(new { Message = "Invalid booking data." }); // Return a 400 Bad Request if the DTO is null
            }

            try
            {
                // Calculate the price asynchronously
                var value = await _bookingService.CalculatePrice(entityDto);
                return Ok(new { TotalPrice = value }); // Return the calculated price in the response
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while calculating the price.");
                return StatusCode(StatusCodes.Status500InternalServerError, new { Message = "An error occurred while processing your request." }); // Handle unexpected errors
            }
        }

        [HttpPost("Book")]
        public async Task<IActionResult> BookingHall([FromBody] BookingDto entityDto)
        {
            // Validate input data
            if (entityDto == null)
            {
                return BadRequest(new { Message = "Booking details must be provided." });
            }

            try
            {
                // Call the service to book the hall and calculate the total price
                var totalPrice = await _bookingService.BookingHall(entityDto);

                // Return confirmation of booking with the total price
                return Ok(new
                {
                    Message = "Booking confirmed.",
                    TotalPrice = totalPrice
                });
            }
            catch (InvalidOperationException ex)
            {
                // Handle specific errors related to booking
                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                // Handle unexpected errors
                _logger.LogError(ex, "An error occurred while booking the hall.");
                return StatusCode(StatusCodes.Status500InternalServerError, new { Message = "An error occurred while processing your request." });
            }
        }
    }
}
