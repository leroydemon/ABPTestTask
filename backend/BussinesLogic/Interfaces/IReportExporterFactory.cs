using BussinesLogic.Enums;

namespace BussinesLogic.Interfaces
{
    public interface IReportExporterFactory
    {
        IReportExporter CreateExporter(ReportFormat format);
    }
}
