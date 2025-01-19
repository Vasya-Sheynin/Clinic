namespace Application.Commands.ReceptionistCommands;
using MediatR;

public record DeleteReceptionistCommand(Guid Id) : IRequest;
