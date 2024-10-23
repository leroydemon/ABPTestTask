using ABPTestTask.Common.Enum;
using ABPTestTask.Common.HallUsageReports;


namespace ABPTestTask.Common.ExporterInterfaces
{
    public interface IReportService
    {
        Task<List<HallUsageReport>> GetHallUsageReport(Report reportRequest);
        Task<string> ExportHallUsageReportToFile(Report reportRequest, ReportFormat format = ReportFormat.Csv);
    }
}
