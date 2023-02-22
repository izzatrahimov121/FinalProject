using AutoMapper;
using IshTap.Business.DTOs.CV;
using IshTap.Core.Entities;

namespace IshTap.Business.Mappers;

public class CvMapper : Profile
{
	public CvMapper()
	{
        CreateMap<CVs, CVDto>().ReverseMap();
    }
}
