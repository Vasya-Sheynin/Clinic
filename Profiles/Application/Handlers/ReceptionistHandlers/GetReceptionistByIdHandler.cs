using Application.Dto.Receptionist;
using MediatR;
using ProfileRepositories;
using AutoMapper;
using Application.Queries.ReceptionistQueries;

namespace Application.Handlers.ReceptionistHandlers;

internal class GetReceptionistByIdHandler : IRequestHandler<GetReceptionistProfileByIdQuery, ReceptionistDto>
{
    private readonly IUnitOfWork _repo;
    private readonly IMapper _mapper;

    public GetReceptionistByIdHandler(IUnitOfWork repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public async Task<ReceptionistDto> Handle(GetReceptionistProfileByIdQuery request, CancellationToken cancellationToken)
    {
        var profile = await _repo.ReceptionistProfileRepo.GetReceptionistProfileAsync(request.Id);

        return _mapper.Map<ReceptionistDto>(profile);
    }
}
