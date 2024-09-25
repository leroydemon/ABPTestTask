using BussinesLogic.Interfaces;
using CsvHelper;
using System.Globalization;
using System.Text;

namespace BussinesLogic.Exporters
{
    public class CsvReportExporter : IReportExporter
    {
        public async Task<string> ExportAsync(List<HallUsageReport> report, string filePath)
        {
            using (var writer = new StreamWriter(filePath, false, Encoding.UTF8))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                await csv.WriteRecordsAsync(report);
            }
            return filePath;
        }
    }
}
