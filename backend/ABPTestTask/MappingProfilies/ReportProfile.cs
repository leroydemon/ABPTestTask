using AutoMapper;

public class ReportProfile : Profile
{
    public ReportProfile()
    {
        CreateMap<Report, ReportDto>()
            .ReverseMap();
    }
}
