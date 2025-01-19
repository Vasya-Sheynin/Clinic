using Application.Dto.Patient;
using MediatR;

namespace Application.Commands.PatientCommands;

public record CreatePatientCommand(CreatePatientDto CreatePatientDto) : IRequest<PatientDto>;
