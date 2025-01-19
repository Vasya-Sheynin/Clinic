using Application.Commands.PatientCommands;
using Application.Dto.Patient;
using AutoMapper;
using MediatR;
using ProfileRepositories;
using Profiles;

namespace Application.Handlers.PatientHandlers;

internal class UpdatePatientHandler : IRequestHandler<UpdatePatientCommand>
{
    private readonly IUnitOfWork _repo;
    private readonly IMapper _mapper;

    public UpdatePatientHandler(IUnitOfWork repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public async Task Handle(UpdatePatientCommand request, CancellationToken cancellationToken)
    {
        var profile = await _repo.PatientProfileRepo.GetPatientProfileAsync(request.Id);
        _mapper.Map<UpdatePatientDto, PatientProfile>(request.UpdatePatientDto, profile);
        await _repo.PatientProfileRepo.UpdatePatientProfileAsync(profile);
        await _repo.SaveAsync();
    }
}
