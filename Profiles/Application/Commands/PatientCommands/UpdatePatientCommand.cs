using Application.Dto.Patient;
using MediatR;

namespace Application.Commands.PatientCommands;

public record UpdatePatientCommand(Guid Id, UpdatePatientDto UpdatePatientDto) : IRequest;
