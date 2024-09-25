using BussinesLogic.Enums;

namespace BussinesLogic.Interfaces
{
    public interface IReportService
    {
        Task<List<HallUsageReport>> GetHallUsageReport(ReportRequest reportRequest);
        Task<string> ExportHallUsageReportToFile(ReportRequest reportRequest, ReportFormat format = ReportFormat.Csv);
    }
}
