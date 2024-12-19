using Application.Dto.Receptionist;
using MediatR;

namespace Application.Queries.ReceptionistQueries;

public record GetReceptionistProfilesQuery() : IRequest<IEnumerable<ReceptionistDto>>;
