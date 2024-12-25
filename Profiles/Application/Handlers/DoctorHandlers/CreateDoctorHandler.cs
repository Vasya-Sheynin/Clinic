using Application.Commands.DoctorCommands;
using Application.Dto.Doctor;
using AutoMapper;
using MediatR;
using ProfileRepositories;
using Profiles;

namespace Application.Handlers.DoctorHandlers;

internal class CreateDoctor : IRequestHandler<CreateDoctorCommand, DoctorDto>
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _repo;

    public CreateDoctor(IMapper mapper, IUnitOfWork repo)
    {
        _mapper = mapper;
        _repo = repo;
    }

    public async Task<DoctorDto> Handle(CreateDoctorCommand request, CancellationToken cancellationToken)
    {
        var profileToCreate = _mapper.Map<DoctorProfile>(request.CreateDoctorDto);
        await _repo.DoctorProfileRepo.CreateDoctorProfileAsync(profileToCreate);
        await _repo.SaveAsync();

        var profileDto = _mapper.Map<DoctorDto>(profileToCreate);
        return profileDto;
    }
}
