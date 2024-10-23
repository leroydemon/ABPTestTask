using ABPTestTask.Common.Enum;

namespace ABPTestTask.Common.ExporterInterfaces
{
    public interface IReportExporterFactory
    {
        IReportExporter CreateExporter(ReportFormat format);
    }
}
