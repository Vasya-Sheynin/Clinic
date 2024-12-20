using Application.Dto.Doctor;
using Application.Filters;
using MediatR;

namespace Application.Queries.DoctorQueries;

public record GetDoctorProfilesQuery(DoctorFilter FilterParams) : IRequest<IEnumerable<DoctorDto>>;
