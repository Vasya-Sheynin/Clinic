using Application.Dto.Patient;
using MediatR;
using ProfileRepositories;
using AutoMapper;
using Application.Queries.PatientQueries;

namespace Application.Handlers.PatientHandlers;

internal class GetReceptionistByIdHandler : IRequestHandler<GetPatientProfileByIdQuery, PatientDto>
{
    private readonly IPatientProfileRepo _repo;
    private readonly IMapper _mapper;

    public GetReceptionistByIdHandler(IPatientProfileRepo repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public async Task<PatientDto> Handle(GetPatientProfileByIdQuery request, CancellationToken cancellationToken)
    {
        var profile = await _repo.GetPatientProfileAsync(request.Id);

        return _mapper.Map<PatientDto>(profile);
    }
}
