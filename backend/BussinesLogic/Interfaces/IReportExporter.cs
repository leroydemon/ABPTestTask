namespace BussinesLogic.Interfaces
{
    public interface IReportExporter
    {
        Task<string> ExportAsync(List<HallUsageReport> report, string filePath);
    }
}
