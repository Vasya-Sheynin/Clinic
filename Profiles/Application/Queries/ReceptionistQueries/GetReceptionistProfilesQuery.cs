using Application.Dto.Receptionist;
using MediatR;
using ProfileRepositories.Pagination;

namespace Application.Queries.ReceptionistQueries;

public record GetReceptionistProfilesQuery(PaginationParams PaginationParams) : IRequest<IEnumerable<ReceptionistDto>>;
