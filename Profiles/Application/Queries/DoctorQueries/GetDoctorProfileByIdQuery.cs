using Application.Dto.Doctor;
using MediatR;

namespace Application.Queries.DoctorQueries;

public record GetDoctorProfileByIdQuery(Guid Id) : IRequest<DoctorDto>;
