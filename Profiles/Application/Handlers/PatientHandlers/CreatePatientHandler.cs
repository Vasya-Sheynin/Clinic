using Application.Commands.PatientCommands;
using Application.Dto.Patient;
using AutoMapper;
using MediatR;
using ProfileRepositories;
using Profiles;

namespace Application.Handlers.PatientHandlers;

internal class CreatePatientHandler : IRequestHandler<CreatePatientCommand, PatientDto>
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _repo;

    public CreatePatientHandler(IMapper mapper, IUnitOfWork repo)
    {
        _mapper = mapper;
        _repo = repo;
    }

    public async Task<PatientDto> Handle(CreatePatientCommand request, CancellationToken cancellationToken)
    {
        var profileToCreate = _mapper.Map<PatientProfile>(request.CreatePatientDto);
        await _repo.PatientProfileRepo.CreatePatientProfileAsync(profileToCreate);
        await _repo.SaveAsync();

        var profileDto = _mapper.Map<PatientDto>(profileToCreate);
        return profileDto;
    }
}
