using AutoMapper;
using IshTap.Business.DTOs.CV;
using IshTap.Business.DTOs.Vacancie;
using IshTap.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IshTap.Business.Mappers;

public class CvMapper : Profile
{
	public CvMapper()
	{
        CreateMap<CVs, CVDto>().ReverseMap();
    }
}
