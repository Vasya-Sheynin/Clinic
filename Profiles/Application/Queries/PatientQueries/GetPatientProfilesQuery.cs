using Application.Dto.Patient;
using Application.Filters;
using MediatR;

namespace Application.Queries.PatientQueries;

public record GetPatientProfilesQuery(PatientFilterParams FilterParams) : IRequest<IEnumerable<PatientDto>>;
