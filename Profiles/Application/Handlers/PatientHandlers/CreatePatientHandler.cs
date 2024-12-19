using Application.Commands.PatientCommands;
using Application.Dto.Patient;
using AutoMapper;
using MediatR;
using ProfileRepositories;
using Profiles;

namespace Application.Handlers.PatientHandlers;

internal class CreateReceptionistHandler : IRequestHandler<CreatePatientCommand, PatientDto>
{
    private readonly IMapper _mapper;
    private readonly IPatientProfileRepo _repo;

    public CreateReceptionistHandler(IMapper mapper, IPatientProfileRepo repo)
    {
        _mapper = mapper;
        _repo = repo;
    }

    public async Task<PatientDto> Handle(CreatePatientCommand request, CancellationToken cancellationToken)
    {
        var profileToCreate = _mapper.Map<PatientProfile>(request.CreatePatientDto);
        await _repo.CreatePatientProfileAsync(profileToCreate);

        var profileDto = _mapper.Map<PatientDto>(profileToCreate);
        return profileDto;
    }
}
