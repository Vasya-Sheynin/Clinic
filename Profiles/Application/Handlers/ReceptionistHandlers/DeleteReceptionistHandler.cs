using Application.Commands.ReceptionistCommands;
using MediatR;
using ProfileRepositories;

namespace Application.Handlers.ReceptionistHandlers;

internal class DeleteReceptionistHandler : IRequestHandler<DeleteReceptionistCommand>
{
    private readonly IReceptionistProfileRepo _repo;

    public DeleteReceptionistHandler(IReceptionistProfileRepo repo)
    {
        _repo = repo;
    }

    public async Task Handle(DeleteReceptionistCommand request, CancellationToken cancellationToken)
    {
        var profile = await _repo.GetReceptionistProfileAsync(request.Id);
        await _repo.DeleteReceptionistProfileAsync(profile);
    }
}
