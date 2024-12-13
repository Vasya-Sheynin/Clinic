using Application.Dto.Doctor;
using AutoMapper;
using Profiles;

namespace Application.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<DoctorProfile, DoctorDto>();
        CreateMap<CreateDoctorDto, DoctorProfile>()
            .AfterMap((src, dest) => dest.Id = Guid.NewGuid());
        CreateMap<CreateDoctorDto, DoctorDto>();
        CreateMap<UpdateDoctorDto, DoctorProfile>();
    }
}
