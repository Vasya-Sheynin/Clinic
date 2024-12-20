using Application.Dto.Patient;
using Application.Filters;
using MediatR;

namespace Application.Queries.PatientQueries;

public record GetPatientProfilesQuery(PatientFilter FilterParams) : IRequest<IEnumerable<PatientDto>>;
