using AutoMapper;
using IshTap.Business.DTOs.GetInTouch;
using IshTap.Core.Entities;

namespace IshTap.Business.Mappers;

public class GetInTouchMapper : Profile
{
	public GetInTouchMapper()
	{
		CreateMap<GetInTouch, GetInTouchDto>().ReverseMap();
	}
}
