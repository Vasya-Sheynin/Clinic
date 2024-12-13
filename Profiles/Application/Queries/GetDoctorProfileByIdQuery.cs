using Application.Dto.Doctor;
using MediatR;

namespace Application.Queries;

public record GetDoctorProfileByIdQuery(Guid Id) : IRequest<DoctorDto>;
