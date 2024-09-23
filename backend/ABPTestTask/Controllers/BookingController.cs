using BussinesLogic.EntitiesDto;
using BussinesLogic.Interfaces;
using Domain.Filters;
using Microsoft.AspNetCore.Mvc;

namespace ABPTestTask.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly IBookingService _bookingService;

        public BookingController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<Booking>>> SearchAsync([FromQuery] BookingFilter filter)
        {
            var entities = await _bookingService.SearchAsync(filter);

            return Ok(entities);
        }
        [HttpDelete]
        public async Task<IActionResult> RemoveAsync(Guid Id)
        {
            await _bookingService.RemoveAsync(Id);

            return Ok();
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(Guid id)
        {
            var entity = await _bookingService.GetByIdAsync(id);

            return Ok(entity);  
        }
        [HttpPut]
        public async Task<IActionResult> UpdateAsync(BookingDto entityDto)
        {
            await _bookingService.UpdateAsync(entityDto);

            return Ok();
        }

        [HttpPost("calculate")]
        public async Task<IActionResult> CalculatePrice(BookingDto entityDto)
        {
            var value = _bookingService.CalculatePrice(entityDto);

            return Ok(value);
        }

        [HttpPost("Book")]
        public async Task<IActionResult> BookingHall(BookingDto entityDto)
        {
            var value = await _bookingService.BookingHall(entityDto);

            return Ok(value);
        }
    }
}
