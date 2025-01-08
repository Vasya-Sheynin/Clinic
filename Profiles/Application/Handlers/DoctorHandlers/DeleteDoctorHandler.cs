using Application.Commands.DoctorCommands;
using MediatR;
using ProfileRepositories;

namespace Application.Handlers.DoctorHandlers;

internal class DeleteDoctorHandler : IRequestHandler<DeleteDoctorCommand>
{
    private readonly IUnitOfWork _repo;

    public DeleteDoctorHandler(IUnitOfWork repo)
    {
        _repo = repo;
    }

    public async Task Handle(DeleteDoctorCommand request, CancellationToken cancellationToken)
    {
        var profile = await _repo.DoctorProfileRepo.GetDoctorProfileAsync(request.Id);
        await _repo.DoctorProfileRepo.DeleteDoctorProfileAsync(profile);
        await _repo.SaveAsync();
    }
}
