﻿using Application.Dto.Patient;
using Application.Queries.PatientQueries;
using AutoMapper;
using MediatR;
using ProfileRepositories;

namespace Application.Handlers.PatientHandlers;

internal class GetPatientsHandler : IRequestHandler<GetPatientProfilesQuery, IEnumerable<PatientDto>>
{
    private readonly IPatientProfileRepo _repo;
    private readonly IMapper _mapper;

    public GetPatientsHandler(IPatientProfileRepo repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public Task<IEnumerable<PatientDto>> Handle(GetPatientProfilesQuery request, CancellationToken cancellationToken)
    {
        var profiles = _repo.GetPatientProfiles(request.FilterParams, request.PaginationParams);
            
        return Task.FromResult(_mapper.Map<IEnumerable<PatientDto>>(profiles));
    }
}
