using Application.Commands.ReceptionistCommands;
using MediatR;
using ProfileRepositories;

namespace Application.Handlers.ReceptionistHandlers;

internal class DeleteReceptionistHandler : IRequestHandler<DeleteReceptionistCommand>
{
    private readonly IUnitOfWork _repo;

    public DeleteReceptionistHandler(IUnitOfWork repo)
    {
        _repo = repo;
    }

    public async Task Handle(DeleteReceptionistCommand request, CancellationToken cancellationToken)
    {
        var profile = await _repo.ReceptionistProfileRepo.GetReceptionistProfileAsync(request.Id);
        await _repo.ReceptionistProfileRepo.DeleteReceptionistProfileAsync(profile);
        await _repo.SaveAsync();
    }
}
