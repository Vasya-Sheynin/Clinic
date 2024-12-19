using MediatR;

namespace Application.Commands.PatientCommands;

public record DeletePatientCommand(Guid Id) : IRequest;
