using Application.Dto;
using Application.Mapper;
using AutoMapper;
using IntegrationTests.OfficeTests.Util;
using Moq;
using Newtonsoft.Json;
using Offices;
using System.Net;
using System.Text;

namespace IntegrationTests.OfficeTests;

[TestFixture]
public class Tests
{
    private OfficeWebApplicationFactory _factory;
    private HttpClient _client;
    private IEnumerable<OfficeDto> _offices;
    private IMapper _mapper;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        var myMapperProfile = new OfficeMapper();
        var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myMapperProfile));
        _mapper = new Mapper(configuration);
    }

    [SetUp]
    public void Setup()
    {
        _factory = new OfficeWebApplicationFactory();
        _client = _factory.CreateClient();

        _offices = [
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

    [TearDown]
    public void CleanUp()
    {
        _client.Dispose();
        _factory.Dispose();
    }

    [TestCase("7f9b3f4a-12e6-4d9c-9b7a-6f8b3d4e9c7f")]
    [TestCase("5d1b9a8a-b8e5-4b0e-81a2-f62b07a8c2c5")]
    public async Task GetOffice_ProducesOk_WhenOfficeExists(Guid id)
    {
        // Arrange
        var url = $"api/office/{id}";
        var actualOffice = _offices.First(x => x.Id == id);

        _factory.CacheServiceMock
            .Setup(c => c.GetOrCreateAsync($"office-{id}", It.IsAny<Func<CancellationToken, ValueTask<Office>>>(), null, null, default))
            .ReturnsAsync(_mapper.Map<Office>(actualOffice));

        // Act
        var response = await _client.GetAsync(url);

        // Assert
        Assert.That(response.StatusCode == HttpStatusCode.OK);
        await Assert.ThatAsync(async () =>
        {
            var office = JsonConvert.DeserializeObject<OfficeDto>(await response.Content.ReadAsStringAsync());

            return office;
        }, Is.EqualTo(_offices.First(o => o.Id == id)));
    }

    [Test]
    public async Task GetOffices_ProducesOk()
    {
        // Arrange
        var url = $"api/office";
        _factory.OfficeRepoMock
            .Setup(x => x.GetOffices())
            .ReturnsAsync(_mapper.Map<IEnumerable<Office>>(_offices));

        // Act
        var responce = await _client.GetAsync(url);
        var responceOffices = JsonConvert.DeserializeObject<IEnumerable<OfficeDto>>(await responce.Content.ReadAsStringAsync());

        // Assert
        Assert.That(HttpStatusCode.OK == responce.StatusCode);
        Assert.That(responceOffices.Count() == _offices.Count());
        Assert.That(() =>
        {
            foreach (var o in responceOffices)
            {
                if (!_offices.Contains(o))
                    return false;
            }

            return true;
        });
    }

    [TestCase("00000000-1234-5678-3214-000000000000")]
    public async Task Delete_ProducesNotFound_WhenOfficeDoesntExist(Guid id)
    {
        // Arrange
        var url = $"api/office/{id}";
        var actualOffice = _offices.FirstOrDefault(x => x.Id == id);
        _factory.OfficeRepoMock.Setup(x => x.GetOffice(It.IsAny<Guid>())).ReturnsAsync(_mapper.Map<Office?>(actualOffice));

        // Act
        var responce = await _client.DeleteAsync(url);

        // Assert
        Assert.That(HttpStatusCode.NotFound == responce.StatusCode);
    }

    [TestCase("7f9b3f4a-12e6-4d9c-9b7a-6f8b3d4e9c7f")]
    public async Task Delete_ProducesNoContent_WhenOfficeExists(Guid id)
    {
        // Arrange
        var url = $"api/office/{id}";
        var actualOffice = _offices.FirstOrDefault(x => x.Id == id);
        _factory.OfficeRepoMock.Setup(x => x.GetOffice(It.IsAny<Guid>())).ReturnsAsync(_mapper.Map<Office>(actualOffice));

        // Act
        var responce = await _client.DeleteAsync(url);

        // Assert
        Assert.That(HttpStatusCode.NoContent == responce.StatusCode);
    }

    [Test, TestCaseSource(typeof(TestCaseSources), nameof(TestCaseSources.UpdateOfficeTestCasesNotFound))]
    public async Task Update_ProducesNotFound_WhenOfficeDoesntExist(Guid id, UpdateOfficeDto updateOfficeDto)
    {
        // Arrange
        var url = $"api/office/{id}";
        var requestBody = new StringContent(
            JsonConvert.SerializeObject(updateOfficeDto),
            Encoding.UTF8,
            "application/json"
            );
        var actualOffice = _offices.FirstOrDefault(x => x.Id == id);
        _factory.OfficeRepoMock.Setup(x => x.GetOffice(It.IsAny<Guid>())).ReturnsAsync(_mapper.Map<Office>(actualOffice));

        // Act
        var response = await _client.PutAsync(url, requestBody);

        // Assert
        Assert.That(HttpStatusCode.NotFound == response.StatusCode);
    }

    [Test, TestCaseSource(typeof(TestCaseSources), nameof(TestCaseSources.UpdateOfficeTestCasesInvalid))]
    public async Task Update_ProducesBadRequest_WhenOfficeInvalid(Guid id, UpdateOfficeDto updateOfficeDto)
    {
        // Arrange
        var url = $"api/office/{id}";
        var requestBody = new StringContent(
            JsonConvert.SerializeObject(updateOfficeDto),
            Encoding.UTF8,
            "application/json"
            );

        // Act
        var response = await _client.PutAsync(url, requestBody);

        // Assert
        Assert.That(HttpStatusCode.BadRequest == response.StatusCode);
    }

    [Test, TestCaseSource(typeof(TestCaseSources), nameof(TestCaseSources.UpdateOfficeTestCasesValid))]
    public async Task Update_ProducesNoContent_WhenOfficeValid(Guid id, UpdateOfficeDto updateOfficeDto)
    {
        // Arrange
        var url = $"api/office/{id}";
        var requestBody = new StringContent(
            JsonConvert.SerializeObject(updateOfficeDto),
            Encoding.UTF8,
            "application/json"
            );
        var actualOffice = _offices.FirstOrDefault(x => x.Id == id);
        _factory.OfficeRepoMock.Setup(x => x.GetOffice(It.IsAny<Guid>())).ReturnsAsync(_mapper.Map<Office>(actualOffice));

        // Act
        var response = await _client.PutAsync(url, requestBody);

        // Assert
        Assert.That(HttpStatusCode.NoContent == response.StatusCode);
    }

    [Test, TestCaseSource(typeof(TestCaseSources), nameof(TestCaseSources.CreateOfficeTestCasesValid))]
    public async Task Create_ProducesCreated_WhenOfficeValid(CreateOfficeDto createOfficeDto)
    {
        // Arrange
        var url = $"api/office";
        var requestBody = new StringContent(
            JsonConvert.SerializeObject(createOfficeDto),
            Encoding.UTF8,
            "application/json"
            );
        var actualOffice = _mapper.Map<Office>(createOfficeDto);

        // Act
        var response = await _client.PostAsync(url, requestBody);
        var responseOffice = JsonConvert.DeserializeObject<Office>(await response.Content.ReadAsStringAsync());

        // Assert
        Assert.That(HttpStatusCode.Created == response.StatusCode);
        Assert.That(actualOffice.Equals(responseOffice));
    }

    [Test, TestCaseSource(typeof(TestCaseSources), nameof(TestCaseSources.CreateOfficeTestCasesInvalid))]
    public async Task Create_ProducesBadRequest_WhenOfficeInvalid(CreateOfficeDto createOfficeDto)
    {
        // Arrange
        var url = $"api/office";
        var requestBody = new StringContent(
            JsonConvert.SerializeObject(createOfficeDto),
            Encoding.UTF8,
            "application/json"
            );

        // Act
        var response = await _client.PostAsync(url, requestBody);

        // Assert
        Assert.That(HttpStatusCode.BadRequest == response.StatusCode);
    }
}
