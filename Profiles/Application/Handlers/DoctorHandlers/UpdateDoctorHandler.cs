using Application.Commands.DoctorCommands;
using Application.Dto.Doctor;
using AutoMapper;
using MediatR;
using ProfileRepositories;
using Profiles;

namespace Application.Handlers.DoctorHandlers;

internal class UpdatePatientHandler : IRequestHandler<UpdateDoctorCommand>
{
    private readonly IDoctorProfileRepo _repo;
    private readonly IMapper _mapper;

    public UpdatePatientHandler(IDoctorProfileRepo repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public async Task Handle(UpdateDoctorCommand request, CancellationToken cancellationToken)
    {
        var profile = await _repo.GetDoctorProfileAsync(request.Id);
        _mapper.Map<UpdateDoctorDto, DoctorProfile>(request.UpdateDoctorDto, profile);
        await _repo.UpdateDoctorProfileAsync(profile);
    }
}
