using Application.Dto.Patient;
using AutoMapper;
using Profiles;

namespace Application.Mapping;

public class PatientMappingProfile : Profile
{
    public PatientMappingProfile()
    {
        CreateMap<PatientProfile, PatientDto>();
        CreateMap<CreatePatientDto, PatientProfile>()
            .AfterMap((src, dest) => dest.Id = Guid.NewGuid());
        CreateMap<CreatePatientDto, PatientDto>();
        CreateMap<UpdatePatientDto, PatientProfile>();
    }
}
