using Application.Dto.Doctor;
using Application.Filters;
using MediatR;

namespace Application.Queries.DoctorQueries;

public record GetDoctorProfilesQuery(DoctorFilterParams FilterParams) : IRequest<IEnumerable<DoctorDto>>;
