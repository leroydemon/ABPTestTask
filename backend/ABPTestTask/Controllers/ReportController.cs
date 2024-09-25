using BussinesLogic.Enums;
using BussinesLogic.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ABPTestTask.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly IReportService _reportService;
        private readonly ILogger<ReportController> _logger;

        public ReportController(IReportService reportService, ILogger<ReportController> logger)
        {
            _reportService = reportService;
            _logger = logger;
        }

        // Get hall usage report based on date range
        [HttpGet("hall-usage")]
        public async Task<ActionResult<List<HallUsageReport>>> GetHallUsageReport([FromQuery] ReportRequest reportRequest)
        {
            if (reportRequest.StartDate > reportRequest.EndDate)
            {
                _logger.LogWarning("Invalid date range provided for hall usage report.");
                return BadRequest("Start date must be earlier than end date.");
            }

            try
            {
                var report = await _reportService.GetHallUsageReport(reportRequest);
                return Ok(report);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while generating the hall usage report.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }
        }

        // Export hall usage report to a specified file format
        [HttpGet("hall-usage/export")]
        public async Task<ActionResult<string>> ExportHallUsageReportToFile([FromQuery] ReportRequest reportRequest, [FromQuery] ReportFormat format = ReportFormat.Csv)
        {
            if (reportRequest.StartDate > reportRequest.EndDate)
            {
                _logger.LogWarning("Invalid date range provided for hall usage export.");
                return BadRequest("Start date must be earlier than end date.");
            }

            try
            {
                var filePath = await _reportService.ExportHallUsageReportToFile(reportRequest, format);
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

