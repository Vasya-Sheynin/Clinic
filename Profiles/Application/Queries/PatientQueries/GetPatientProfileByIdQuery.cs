using Application.Dto.Patient;
using MediatR;

namespace Application.Queries.PatientQueries;

public record GetPatientProfileByIdQuery(Guid Id) : IRequest<PatientDto>;
