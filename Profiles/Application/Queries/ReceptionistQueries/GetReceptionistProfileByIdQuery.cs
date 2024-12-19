using Application.Dto.Receptionist;
using MediatR;

namespace Application.Queries.ReceptionistQueries;

public record GetReceptionistProfileByIdQuery(Guid Id) : IRequest<ReceptionistDto>;

