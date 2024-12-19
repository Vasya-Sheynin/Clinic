using Application.Dto.Patient;
using Application.Queries.PatientQueries;
using AutoMapper;
using MediatR;
using ProfileRepositories;

namespace Application.Handlers.PatientHandlers;

internal class GetReceptionistsHandler : IRequestHandler<GetPatientProfilesQuery, IEnumerable<PatientDto>>
{
    private readonly IPatientProfileRepo _repo;
    private readonly IMapper _mapper;

    public GetReceptionistsHandler(IPatientProfileRepo repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public Task<IEnumerable<PatientDto>> Handle(GetPatientProfilesQuery request, CancellationToken cancellationToken)
    {
        var profiles = _repo.GetPatientProfiles()
            .Where(p =>
            {
                var fullName = $"{p.FirstName} {p.LastName} {p.MiddleName}";
                return request.FilterParams.FullName is null || request.FilterParams.FullName == fullName ;
            });

        return Task.FromResult(_mapper.Map<IEnumerable<PatientDto>>(profiles));
    }
}
