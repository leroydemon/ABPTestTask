using ABPTestTask.Common.ExporterInterfaces;
using Infrastucture.ReportFormat;

namespace BussinesLogic.Exporters
{
    public class ReportExporterFactory : IReportExporterFactory
    {
        public IReportExporter CreateExporter(ReportFormat format)
        {
            return format switch
            {
                ReportFormat.Csv => new CsvReportExporter(),
                ReportFormat.Json => new JsonReportExporter(),
                _ => throw new ArgumentException("Unsupported format")
            };
        }
    }
}
