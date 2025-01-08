using Application.Dto.Patient;
using MediatR;
using ProfileRepositories;
using ProfileRepositories.Pagination;

namespace Application.Queries.PatientQueries;

public record GetPatientProfilesQuery(
    PatientFilter FilterParams, PaginationParams PaginationParams) : IRequest<IEnumerable<PatientDto>>;
