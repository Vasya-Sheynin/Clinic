using Application.Dto.Doctor;
using MediatR;
using ProfileRepositories;
using AutoMapper;
using Application.Queries.DoctorQueries;

namespace Application.Handlers.DoctorHandlers;

internal class GetDoctorByIdHandler : IRequestHandler<GetDoctorProfileByIdQuery, DoctorDto>
{
    private readonly IUnitOfWork _repo;
    private readonly IMapper _mapper;

    public GetDoctorByIdHandler(IUnitOfWork repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public async Task<DoctorDto> Handle(GetDoctorProfileByIdQuery request, CancellationToken cancellationToken)
    {
        var profile = await _repo.DoctorProfileRepo.GetDoctorProfileAsync(request.Id);

        return _mapper.Map<DoctorDto>(profile);
    }
}
