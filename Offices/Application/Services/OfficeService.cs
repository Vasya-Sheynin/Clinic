using Offices;
using OfficeRepositories;
using Application.Dto;
using AutoMapper;
using Application.Exceptions;
using FluentValidation;

namespace Application;

public class OfficeService : IOfficeService
{
    private readonly IOfficeRepository _officeRepository;
    private readonly IMapper _mapper;
    private readonly IValidator<CreateOfficeDto> _createOfficeDtoValidator;
    private readonly IValidator<UpdateOfficeDto> _updateOfficeDtoValidator;

    public OfficeService(
        IOfficeRepository officeRepository, 
        IMapper mapper,
        IValidator<CreateOfficeDto> createOfficeDtoValidator,
        IValidator<UpdateOfficeDto> updateOfficeDtoValidator
        )
    {
        _officeRepository = officeRepository;
        _mapper = mapper;
        _createOfficeDtoValidator = createOfficeDtoValidator;
        _updateOfficeDtoValidator = updateOfficeDtoValidator;
    }

    public async Task<OfficeDto> CreateOffice(CreateOfficeDto createOfficeDto)
    {
        _createOfficeDtoValidator.ValidateAndThrow(createOfficeDto);

        var officeToCreate = _mapper.Map<Office>(createOfficeDto);

        await _officeRepository.CreateOffice(officeToCreate);

        return _mapper.Map<OfficeDto>(officeToCreate);
    }

    public async Task DeleteOffice(Guid id)
    {
        var officeToDelete = await _officeRepository.GetOffice(id);
        if (officeToDelete == null)
            throw new OfficeNotFoundException("Office with provided id wasn't found");

        await _officeRepository.DeleteOffice(officeToDelete);
    }

    public async Task<OfficeDto?> GetOffice(Guid id)
    {
        var office = await _officeRepository.GetOffice(id);

        return _mapper.Map<OfficeDto?>(office);
    }

    public async Task<IEnumerable<OfficeDto>?> GetOffices()
    {
        var offices = await _officeRepository.GetOffices();

        return _mapper.Map<IEnumerable<OfficeDto>?>(offices);
    }

    public async Task UpdateOffice(Guid id, UpdateOfficeDto updateOfficeDto)
    {
        _updateOfficeDtoValidator.ValidateAndThrow(updateOfficeDto);

        var officeToUpdate = await _officeRepository.GetOffice(id);
        if (officeToUpdate == null)
            throw new OfficeNotFoundException("Office with provided id wasn't found");

        _mapper.Map(updateOfficeDto, officeToUpdate);

        await _officeRepository.UpdateOffice(officeToUpdate);
    }
}
