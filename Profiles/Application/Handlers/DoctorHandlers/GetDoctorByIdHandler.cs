using Application.Dto.Doctor;
using MediatR;
using ProfileRepositories;
using AutoMapper;
using Application.Queries.DoctorQueries;

namespace Application.Handlers.DoctorHandlers;

internal class GetPatientByIdHandler : IRequestHandler<GetDoctorProfileByIdQuery, DoctorDto>
{
    private readonly IDoctorProfileRepo _repo;
    private readonly IMapper _mapper;

    public GetPatientByIdHandler(IDoctorProfileRepo repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public async Task<DoctorDto> Handle(GetDoctorProfileByIdQuery request, CancellationToken cancellationToken)
    {
        var profile = await _repo.GetDoctorProfileAsync(request.Id);

        return _mapper.Map<DoctorDto>(profile);
    }
}
