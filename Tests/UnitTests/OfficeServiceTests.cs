using Application;
using Application.Dto;
using Application.Exceptions;
using Application.Mapper;
using Application.Services;
using Application.Validators;
using AutoMapper;
using FluentValidation;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Moq;
using OfficeRepositories;
using Offices;
using System;
using UnitTests.Util;

namespace UnitTests;

[TestFixture]
public class OfficeServiceTests
{
    private IMapper _mapper;
    private Mock<IOfficeRepository> _officeRepo;
    private IValidator<CreateOfficeDto> _createValidator;
    private IValidator<UpdateOfficeDto> _updateValidator;
    private Mock<ICachingService> _cache;
    private OfficeService _officeService;
    private List<Office> _offices;

    [SetUp]
    public void Setup()
    {
        var myMapperProfile = new OfficeMapper();
        var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myMapperProfile));
        _mapper = new Mapper(configuration);

        _createValidator = new CreateOfficeDtoValidator();
        _updateValidator = new UpdateOfficeDtoValidator();

        _officeRepo = new Mock<IOfficeRepository>();
        _cache = new Mock<ICachingService>();

        _officeService = new OfficeService(
            _officeRepo.Object,
            _mapper,
            _createValidator,
            _updateValidator,
            _cache.Object
            );

        _offices = [
            new Office
            {
                Id = Guid.Parse("5d1b9a8a-b8e5-4b0e-81a2-f62b07a8c2c5"),
                PhotoId = Guid.NewGuid(),
                Address = "Address1",
                RegistryPhoneNumber = "+123456",
                IsActive = false
            },
            new Office
            {
                Id = Guid.Parse("3e7fc598-b464-48ed-9c8e-d23eff459edd"),
                PhotoId = Guid.NewGuid(),
                Address = "Address2",
                RegistryPhoneNumber = "+789984",
                IsActive = false
            },
            new Office
            {
                Id = Guid.Parse("7f9b3f4a-12e6-4d9c-9b7a-6f8b3d4e9c7f"),
                PhotoId = Guid.NewGuid(),
                Address = "Address3",
                RegistryPhoneNumber = "+65496",
                IsActive = true
            },
       ];
    }

    [Test]
    public async Task GetOffices_ReturnsExpected()
    {
        // Arrange
        _officeRepo.Setup(x => x.GetOffices()).ReturnsAsync(_offices);

        // Action
        var receivedOffices = await _officeService.GetOffices();

        // Assert
        Assert.That(receivedOffices.Count() == _offices.Count);
        Assert.That(() =>
        {
            foreach (var o in receivedOffices)
            {
                if (!_offices.Contains(_mapper.Map<Office>(o), new OfficeComparer()))
                    return false;
            }

            return true;
        });
    }

    [TestCase("7f9b3f4a-12e6-4d9c-9b7a-6f8b3d4e9c7f")]
    [TestCase("5d1b9a8a-b8e5-4b0e-81a2-f62b07a8c2c5")]
    public async Task GetOffice_ReturnsExpected_WhenOfficeExists(Guid id)
    {
        // Arrange
        var actualOffice = _offices.First(x => x.Id == id);

        _cache
            .Setup(c => c.GetOrCreateAsync($"office-{id}", It.IsAny<Func<CancellationToken, ValueTask<Office>>>(), null, null, default))
            .ReturnsAsync(actualOffice);

        // Action
        var receivedOffice = await _officeService.GetOffice(id);

        // Assert
        Assert.That(() => receivedOffice is not null && receivedOffice.Id == id);
    }

    [TestCase("00000000-1234-5678-3214-000000000000")]
    public async Task Delete_Throws_WhenOfficeDoesntExist(Guid id)
    {
        // Arrange
        var actualOffice = _offices.FirstOrDefault(x => x.Id == id);
        _officeRepo.Setup(x => x.GetOffice(It.IsAny<Guid>())).ReturnsAsync(actualOffice);

        // Assert
        Assert.ThrowsAsync<OfficeNotFoundException>(async () => await _officeService.DeleteOffice(id));
    }

    [TestCase("7f9b3f4a-12e6-4d9c-9b7a-6f8b3d4e9c7f")]
    public void Delete_DoesNotThrow_WhenOfficeExists(Guid id)
    {
        // Arrange
        var actualOffice = _offices.FirstOrDefault(x => x.Id == id);
        _officeRepo.Setup(x => x.GetOffice(It.IsAny<Guid>())).ReturnsAsync(actualOffice);

        // Assert
        Assert.DoesNotThrowAsync(async () => await _officeService.DeleteOffice(id));
    }

    [TestCaseSource(typeof(TestCaseSources), nameof(TestCaseSources.UpdateOfficeTestCasesNotFound))]
    public void Update_Throws_WhenOfficeDoesntExist(Guid id, UpdateOfficeDto updateOfficeDto)
    {
        // Arrange
        var actualOffice = _offices.FirstOrDefault(x => x.Id == id);
        _officeRepo.Setup(x => x.GetOffice(It.IsAny<Guid>())).ReturnsAsync(actualOffice);

        // Assert
        Assert.ThrowsAsync<OfficeNotFoundException>(async () => await _officeService.UpdateOffice(id, updateOfficeDto));
    }

    [TestCaseSource(typeof(TestCaseSources), nameof(TestCaseSources.UpdateOfficeTestCasesInvalid))]
    public void Update_Throws_WhenOfficeInvalid(Guid id, UpdateOfficeDto updateOfficeDto)
    {
        //Assert
        Assert.ThrowsAsync<ValidationException>(async () => await _officeService.UpdateOffice(id, updateOfficeDto));
    }

    [TestCaseSource(typeof(TestCaseSources), nameof(TestCaseSources.CreateOfficeTestCasesInvalid))]
    public void Create_ReturnsExpected_WhenOfficeValid(CreateOfficeDto createOfficeDto)
    {
        // Assert
        Assert.ThrowsAsync<ValidationException>(async () => await _officeService.CreateOffice(createOfficeDto));
    }
}
