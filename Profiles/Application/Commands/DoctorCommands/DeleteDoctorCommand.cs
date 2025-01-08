using MediatR;

namespace Application.Commands.DoctorCommands;

public record DeleteDoctorCommand(Guid Id) : IRequest;