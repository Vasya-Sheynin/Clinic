using Application.Dto.Receptionist;
using MediatR;

namespace Application.Commands.ReceptionistCommands;

public record UpdateReceptionistCommand(Guid Id, UpdateReceptionistDto UpdateReceptionistDto) : IRequest;
