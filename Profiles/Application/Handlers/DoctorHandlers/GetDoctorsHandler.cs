using Application.Dto.Doctor;
using Application.Queries.DoctorQueries;
using AutoMapper;
using MediatR;
using ProfileRepositories;

namespace Application.Handlers.DoctorHandlers;

internal class GetPatientsHandler : IRequestHandler<GetDoctorProfilesQuery, IEnumerable<DoctorDto>>
{
    private readonly IDoctorProfileRepo _repo;
    private readonly IMapper _mapper;

    public GetPatientsHandler(IDoctorProfileRepo repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public Task<IEnumerable<DoctorDto>> Handle(GetDoctorProfilesQuery request, CancellationToken cancellationToken)
    {
        var profiles = _repo.GetDoctorProfiles()
            .Where(p => (request.FilterParams.SpecializationId is null || p.SpecializationId == request.FilterParams.SpecializationId) &&
                (request.FilterParams.OfficeId is null || p.OfficeId == request.FilterParams.OfficeId))
            .OrderBy(p => $"{p.FirstName} {p.LastName} {p.MiddleName}");

        return Task.FromResult(_mapper.Map<IEnumerable<DoctorDto>>(profiles));
    }
}
