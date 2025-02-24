using Application;
using Application.Dto;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OfficesController.Controllers;
using Microsoft.AspNetCore.Mvc;
using Application.Exceptions;

namespace UnitTests.OfficeTests;

[TestFixture]
public class OfficeControllerTests
{
    private IEnumerable<OfficeDto> _officeDtos;
    private Mock<IOfficeService> _officeServiceMock;
    private OfficesController.Controllers.OfficesController _officesController;

    [SetUp]
    public void SetUp()
    {
        _officeServiceMock = new Mock<IOfficeService>();
        _officesController = new OfficesController.Controllers.OfficesController(_officeServiceMock.Object); 

        _officeDtos = [
            new OfficeDto
            (
                Guid.Parse("5d1b9a8a-b8e5-4b0e-81a2-f62b07a8c2c5"),
                Guid.NewGuid(),
                "Address1",
                "+123456",
                false
            ),
            new OfficeDto
            (
                Guid.Parse("3e7fc598-b464-48ed-9c8e-d23eff459edd"),
                Guid.NewGuid(),
                "Address2",
                "+789984",
                false
            ),
            new OfficeDto
            (
                Guid.Parse("7f9b3f4a-12e6-4d9c-9b7a-6f8b3d4e9c7f"),
                Guid.NewGuid(),
                "Address3",
                "+65496",
                true
            ),
       ];
    }

    [Test]
    public async Task GetOffices_ProducesOk()
    {
        // Arrange
        _officeServiceMock
            .Setup(x => x.GetOffices())
            .ReturnsAsync(_officeDtos);

        // Act
        var result = await _officesController.GetOffices();
        var receivedOffices = (result.Result as ObjectResult).Value as IEnumerable<OfficeDto>;

        // Assert
        Assert.That(result.Result.GetType() == typeof(OkObjectResult));
        Assert.That(receivedOffices.Count() == _officeDtos.Count());
        Assert.That(() => 
        {
            foreach (var office in receivedOffices)
            {
                if (!_officeDtos.Contains(office)) { return false; }
            }

            return true;
        });
    }

    [TestCase("3e7fc598-b464-48ed-9c8e-d23eff459edd")]
    public async Task GetOffice_ProducesOk_WhenOfficeExists(Guid id)
    {
        // Arrange
        var actualOffice = _officeDtos.First(x => x.Id == id);
        _officeServiceMock
            .Setup(x => x.GetOffice(It.IsAny<Guid>()))
            .ReturnsAsync(actualOffice);

        // Act
        var result = await _officesController.GetOffice(id);
        var receivedOffice = (result.Result as ObjectResult).Value as OfficeDto;

        // Assert
        Assert.That(receivedOffice == actualOffice);
    }

    [TestCase("09101000-1234-5678-3214-000072001570")]
    public void GetOffice_ProducesNotFound_WhenOfficeDoesNotExist(Guid id)
    {
        // Arrange
        _officeServiceMock
            .Setup(x => x.GetOffice(It.IsAny<Guid>()))
            .ThrowsAsync(new OfficeNotFoundException("Office with provided id wasn't found"));

        // Assert
        Assert.ThrowsAsync<OfficeNotFoundException>(async () => await _officesController.GetOffice(id));
    }
}
