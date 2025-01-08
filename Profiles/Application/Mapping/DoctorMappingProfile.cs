using Application.Dto.Doctor;
using AutoMapper;
using Profiles;

namespace Application.Mapping;

public class DoctorMappingProfile : Profile
{
    public DoctorMappingProfile()
    {
        CreateMap<DoctorProfile, DoctorDto>();
        CreateMap<CreateDoctorDto, DoctorProfile>()
            .AfterMap((src, dest) => dest.Id = Guid.NewGuid());
        CreateMap<CreateDoctorDto, DoctorDto>();
        CreateMap<UpdateDoctorDto, DoctorProfile>();
    }
}
