using Application.Commands.ReceptionistCommands;
using Application.Dto.Receptionist;
using AutoMapper;
using MediatR;
using ProfileRepositories;
using Profiles;

namespace Application.Handlers.ReceptionistHandlers;

internal class UpdateReceptionistHandler : IRequestHandler<UpdateReceptionistCommand>
{
    private readonly IReceptionistProfileRepo _repo;
    private readonly IMapper _mapper;

    public UpdateReceptionistHandler(IReceptionistProfileRepo repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public async Task Handle(UpdateReceptionistCommand request, CancellationToken cancellationToken)
    {
        var profile = await _repo.GetReceptionistProfileAsync(request.Id);
        _mapper.Map<UpdateReceptionistDto, ReceptionistProfile>(request.UpdateReceptionistDto, profile);
        await _repo.UpdateReceptionistProfileAsync(profile);
    }
}
