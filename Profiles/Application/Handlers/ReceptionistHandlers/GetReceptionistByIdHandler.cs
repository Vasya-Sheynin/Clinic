using Application.Dto.Receptionist;
using MediatR;
using ProfileRepositories;
using AutoMapper;
using Application.Queries.ReceptionistQueries;

namespace Application.Handlers.ReceptionistHandlers;

internal class GetReceptionistByIdHandler : IRequestHandler<GetReceptionistProfileByIdQuery, ReceptionistDto>
{
    private readonly IReceptionistProfileRepo _repo;
    private readonly IMapper _mapper;

    public GetReceptionistByIdHandler(IReceptionistProfileRepo repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public async Task<ReceptionistDto> Handle(GetReceptionistProfileByIdQuery request, CancellationToken cancellationToken)
    {
        var profile = await _repo.GetReceptionistProfileAsync(request.Id);

        return _mapper.Map<ReceptionistDto>(profile);
    }
}
