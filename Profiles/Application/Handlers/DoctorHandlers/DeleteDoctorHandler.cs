using Application.Commands.DoctorCommands;
using MediatR;
using ProfileRepositories;

namespace Application.Handlers.DoctorHandlers;

internal class DeletePatientHandler : IRequestHandler<DeleteDoctorCommand>
{
    private readonly IDoctorProfileRepo _repo;

    public DeletePatientHandler(IDoctorProfileRepo repo)
    {
        _repo = repo;
    }

    public async Task Handle(DeleteDoctorCommand request, CancellationToken cancellationToken)
    {
        var profile = await _repo.GetDoctorProfileAsync(request.Id);
        await _repo.DeleteDoctorProfileAsync(profile);
    }
}
