using Application.Dto.Doctor;
using MediatR;

namespace Application.Queries;

public class GetDoctorProfilesQuery() : IRequest<IEnumerable<DoctorDto>>;
