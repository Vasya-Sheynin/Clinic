﻿using Application.Dto.Receptionist;
using Application.Queries.ReceptionistQueries;
using AutoMapper;
using MediatR;
using ProfileRepositories;

namespace Application.Handlers.ReceptionistHandlers;

internal class GetReceptionistsHandler : IRequestHandler<GetReceptionistProfilesQuery, IEnumerable<ReceptionistDto>>
{
    private readonly IUnitOfWork _repo;
    private readonly IMapper _mapper;

    public GetReceptionistsHandler(IUnitOfWork repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public Task<IEnumerable<ReceptionistDto>> Handle(GetReceptionistProfilesQuery request, CancellationToken cancellationToken)
    {
        var profiles = _repo.ReceptionistProfileRepo.GetReceptionistProfiles(request.PaginationParams);

        return Task.FromResult(_mapper.Map<IEnumerable<ReceptionistDto>>(profiles));
    }
}
