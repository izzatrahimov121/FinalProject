using AutoMapper;
using IshTap.Business.DTOs.Vacancie;
using IshTap.Core.Entities;

namespace IshTap.Business.Mappers;

public class VacancieMapper : Profile
{
    public VacancieMapper()
    {
        CreateMap<Vacancie, VacancieDto>().ReverseMap();
    }
}
