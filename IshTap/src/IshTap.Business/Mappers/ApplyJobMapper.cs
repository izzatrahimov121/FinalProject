using AutoMapper;
using IshTap.Business.DTOs.ApplyJob;
using IshTap.Core.Entities;

namespace IshTap.Business.Mappers;

public class ApplyJobMapper : Profile
{
	public ApplyJobMapper()
	{
		CreateMap<ApplyJob, ApplyJobDto>().ReverseMap();
	}
}
