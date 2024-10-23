using System.Text.Json;
using System.Text;
using ABPTestTask.Common.ExporterInterfaces;
using ABPTestTask.Common.HallUsageReport;

namespace BussinesLogic.Exporters
{
    public class JsonReportExporter : IReportExporter
    {
        public async Task<string> ExportAsync(List<HallUsageReport> report, string filePath)
        {
            using (var writer = new StreamWriter(filePath, false, Encoding.UTF8))
            {
                var json = JsonSerializer.Serialize(report);
                await writer.WriteAsync(json);
            }
            return filePath;
        }
    }
}
