using Application.Commands.PatientCommands;
using Application.Dto.Patient;
using AutoMapper;
using MediatR;
using ProfileRepositories;
using Profiles;

namespace Application.Handlers.PatientHandlers;

internal class UpdateReceptionistHandler : IRequestHandler<UpdatePatientCommand>
{
    private readonly IPatientProfileRepo _repo;
    private readonly IMapper _mapper;

    public UpdateReceptionistHandler(IPatientProfileRepo repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public async Task Handle(UpdatePatientCommand request, CancellationToken cancellationToken)
    {
        var profile = await _repo.GetPatientProfileAsync(request.Id);
        _mapper.Map<UpdatePatientDto, PatientProfile>(request.UpdatePatientDto, profile);
        await _repo.UpdatePatientProfileAsync(profile);
    }
}
