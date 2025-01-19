using Application.Dto.Receptionist;
using MediatR;

namespace Application.Commands.ReceptionistCommands;

public record CreateReceptionistCommand(CreateReceptionistDto CreateReceptionistDto) : IRequest<ReceptionistDto>;
