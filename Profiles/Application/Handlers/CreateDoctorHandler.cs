using Application.Commands;
using Application.Dto.Doctor;
using AutoMapper;
using MediatR;
using ProfileRepositories;
using Profiles;

namespace Application.Handlers;

internal class CreateDoctorHandler : IRequestHandler<CreateDoctorCommand, DoctorDto>
{
    private readonly IMapper _mapper;
    private readonly IDoctorProfileRepo _repo;

    public CreateDoctorHandler(IMapper mapper, IDoctorProfileRepo repo)
    {
        _mapper = mapper;
        _repo = repo;
    }

    public async Task<DoctorDto> Handle(CreateDoctorCommand request, CancellationToken cancellationToken)
    {
        var profileToCreate = _mapper.Map<DoctorProfile>(request.DoctorDto);
        await _repo.CreateDoctorProfileAsync(profileToCreate);

        var profileDto = _mapper.Map<DoctorDto>(profileToCreate);
        return profileDto;
    }
}
