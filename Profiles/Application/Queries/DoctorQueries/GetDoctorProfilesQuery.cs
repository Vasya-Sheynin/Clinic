using Application.Dto.Doctor;
using MediatR;
using ProfileRepositories;
using ProfileRepositories.Pagination;

namespace Application.Queries.DoctorQueries;

public record GetDoctorProfilesQuery(
    DoctorFilter FilterParams, PaginationParams PaginationParams) : IRequest<IEnumerable<DoctorDto>>;
