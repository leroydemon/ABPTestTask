using ABPTestTask.Common.Enum;
using ABPTestTask.Common.ExporterInterfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace ABPTestTask.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly IReportService _reportService;
        private readonly ILogger<ReportController> _logger;
        private readonly IMapper _mapper;

        public ReportController(IReportService reportService, ILogger<ReportController> logger, IMapper mapper)
        {
            _reportService = reportService;
            _logger = logger;
            _mapper = mapper;
        }

        // Get hall usage report based on date range
        [HttpGet("hall-usage")]
        public async Task<ActionResult<List<HallUsageReportDto>>> GetHallUsageReport([FromQuery] ReportDto reportRequest)
        {
            if (reportRequest.StartDate > reportRequest.EndDate)
            {
                _logger.LogWarning("Invalid date range provided for hall usage report.");
                return BadRequest("Start date must be earlier than end date.");
            }

            try
            {
                var report = await _reportService.GetHallUsageReport(_mapper.Map<Report>(reportRequest));

                return Ok(_mapper.Map<ReportDto>(report));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while generating the hall usage report.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }
        }

        // Export hall usage report to a specified file format
        [HttpGet("hall-usage/export")]
        public async Task<ActionResult<string>> ExportHallUsageReportToFile([FromQuery] ReportDto reportRequest, [FromQuery] ReportFormat format = ReportFormat.Csv)
        {
            if (reportRequest.StartDate > reportRequest.EndDate)
            {
                _logger.LogWarning("Invalid date range provided for hall usage export.");
                return BadRequest("Start date must be earlier than end date.");
            }

            try
            {
                var filePath = await _reportService.ExportHallUsageReportToFile(_mapper.Map<Report>(reportRequest), format);
                return Ok(new { FilePath = filePath });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while exporting the hall usage report.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }
        }
    }
}

