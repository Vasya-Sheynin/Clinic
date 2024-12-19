using Application.Dto.Receptionist;
using AutoMapper;
using Profiles;

namespace Application.Mapping;

public class ReceptionistMappingProfile : Profile
{
    public ReceptionistMappingProfile()
    {
        CreateMap<ReceptionistProfile, ReceptionistDto>();
        CreateMap<CreateReceptionistDto, ReceptionistProfile>()
            .AfterMap((src, dest) => dest.Id = Guid.NewGuid());
        CreateMap<CreateReceptionistDto, ReceptionistDto>();
        CreateMap<UpdateReceptionistDto, ReceptionistProfile>();
    }
}
