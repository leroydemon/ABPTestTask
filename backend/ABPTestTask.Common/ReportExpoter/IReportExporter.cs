using ABPTestTask.Common.HallUsageReports;

namespace ABPTestTask.Common.ExporterInterfaces
{
    public interface IReportExporter
    {
        Task<string> ExportAsync(List<HallUsageReport> report, string filePath);
    }
}
