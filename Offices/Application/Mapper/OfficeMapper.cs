using Application.Dto;
using AutoMapper;
using Offices;

namespace Application.Mapper;

public class OfficeMapper : Profile
{
    public OfficeMapper()
    {
        CreateMap<CreateOfficeDto, Office>()
            .AfterMap((src, dest) => dest.Id = Guid.NewGuid());
        CreateMap<UpdateOfficeDto, Office>();
        CreateMap<OfficeDto, Office>();
        CreateMap<Office, OfficeDto>();
    }
}
