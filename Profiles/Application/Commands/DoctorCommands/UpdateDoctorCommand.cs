using Application.Dto.Doctor;
using MediatR;

namespace Application.Commands.DoctorCommands;

public record UpdateDoctorCommand(Guid Id, UpdateDoctorDto UpdateDoctorDto) : IRequest;
