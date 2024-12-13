using Application.Dto.Doctor;
using MediatR;

namespace Application.Commands;

public record UpdateDoctorCommand(Guid Id, UpdateDoctorDto UpdateDoctorDto) : IRequest;
