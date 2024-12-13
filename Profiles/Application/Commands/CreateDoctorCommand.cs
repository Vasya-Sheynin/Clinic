using Application.Dto.Doctor;
using MediatR;

namespace Application.Commands;

public record CreateDoctorCommand(CreateDoctorDto DoctorDto) : IRequest<DoctorDto>;
