using Application.Queries;
using Application.Dto.Doctor;
using MediatR;
using ProfileRepositories;
using AutoMapper;

namespace Application.Handlers;

internal class GetDoctorByIdHandler : IRequestHandler<GetDoctorProfileByIdQuery, DoctorDto>
{
    private readonly IDoctorProfileRepo _repo;
    private readonly IMapper _mapper;

    public GetDoctorByIdHandler(IDoctorProfileRepo repo, IMapper mapper)
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
