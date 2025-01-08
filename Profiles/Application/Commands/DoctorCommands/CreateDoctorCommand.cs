using Application.Dto.Doctor;
using MediatR;

namespace Application.Commands.DoctorCommands;

public record CreateDoctorCommand(CreateDoctorDto CreateDoctorDto) : IRequest<DoctorDto>;
