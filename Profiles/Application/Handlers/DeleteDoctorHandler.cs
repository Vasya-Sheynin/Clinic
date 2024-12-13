using Application.Commands;
using MediatR;
using ProfileRepositories;

namespace Application.Handlers;

internal class DeleteDoctorHandler : IRequestHandler<DeleteDoctorCommand>
{
    private readonly IDoctorProfileRepo _repo;

    public DeleteDoctorHandler(IDoctorProfileRepo repo)
    {
        _repo = repo;
    }

    public async Task Handle(DeleteDoctorCommand request, CancellationToken cancellationToken)
    {
        var profile = await _repo.GetDoctorProfileAsync(request.Id);
        await _repo.DeleteDoctorProfileAsync(profile);
    }
}
