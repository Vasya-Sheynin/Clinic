using Application.Commands.ReceptionistCommands;
using Application.Dto.Receptionist;
using AutoMapper;
using MediatR;
using ProfileRepositories;
using Profiles;

namespace Application.Handlers.ReceptionistHandlers;

internal class CreateReceptionistHandler : IRequestHandler<CreateReceptionistCommand, ReceptionistDto>
{
    private readonly IMapper _mapper;
    private readonly IReceptionistProfileRepo _repo;

    public CreateReceptionistHandler(IMapper mapper, IReceptionistProfileRepo repo)
    {
        _mapper = mapper;
        _repo = repo;
    }

    public async Task<ReceptionistDto> Handle(CreateReceptionistCommand request, CancellationToken cancellationToken)
    {
        var profileToCreate = _mapper.Map<ReceptionistProfile>(request.CreateReceptionistDto);
        await _repo.CreateReceptionistProfileAsync(profileToCreate);

        var profileDto = _mapper.Map<ReceptionistDto>(profileToCreate);
        return profileDto;
    }
}
