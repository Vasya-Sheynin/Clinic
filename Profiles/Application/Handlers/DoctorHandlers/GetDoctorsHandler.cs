using Application.Dto.Doctor;
using Application.Queries.DoctorQueries;
using AutoMapper;
using MediatR;
using ProfileRepositories;

namespace Application.Handlers.DoctorHandlers;

internal class GetDoctorsHandler : IRequestHandler<GetDoctorProfilesQuery, IEnumerable<DoctorDto>>
{
    private readonly IUnitOfWork _repo;
    private readonly IMapper _mapper;

    public GetDoctorsHandler(IUnitOfWork repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public Task<IEnumerable<DoctorDto>> Handle(GetDoctorProfilesQuery request, CancellationToken cancellationToken)
    {
        var profiles = _repo.DoctorProfileRepo.GetDoctorProfiles(request.FilterParams, request.PaginationParams);

        return Task.FromResult(_mapper.Map<IEnumerable<DoctorDto>>(profiles));
    }
}
