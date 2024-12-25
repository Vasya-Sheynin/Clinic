using Application.Commands.PatientCommands;
using MediatR;
using ProfileRepositories;

namespace Application.Handlers.PatientHandlers;

internal class DeletePatientHandler : IRequestHandler<DeletePatientCommand>
{
    private readonly IUnitOfWork _repo;

    public DeletePatientHandler(IUnitOfWork repo)
    {
        _repo = repo;
    }

    public async Task Handle(DeletePatientCommand request, CancellationToken cancellationToken)
    {
        var profile = await _repo.PatientProfileRepo.GetPatientProfileAsync(request.Id);
        await _repo.PatientProfileRepo.DeletePatientProfileAsync(profile);
        await _repo.SaveAsync();
    }
}
