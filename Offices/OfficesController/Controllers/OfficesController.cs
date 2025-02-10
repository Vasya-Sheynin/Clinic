using Application;
using Application.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Offices;

namespace OfficesController.Controllers;

[Route("api/office")]
[ApiController]
public class OfficesController : ControllerBase
{
    private readonly IOfficeService _officeService;

    public OfficesController(IOfficeService officeService)
    {
        _officeService = officeService;
    }

    [Authorize(Roles = "Receptionist")]
    [HttpGet("{id}")]
    public async Task<ActionResult<Office>> GetOffice([FromRoute] Guid id)
    {
        var office = await _officeService.GetOffice(id);

        return Ok(office);
    }

    [Authorize(Roles = "Receptionist")]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Office>>> GetOffices()
    {
        var offices = await _officeService.GetOffices();

        return Ok(offices);
    }
    
    [Authorize(Roles = "Receptionist")]
    [HttpPost]
    public async Task<ActionResult> CreateOffice([FromBody] CreateOfficeDto createOfficeDto)
    {
        var office = await _officeService.CreateOffice(createOfficeDto);

        return CreatedAtAction(nameof(GetOffice), new { office.Id }, office);
    }

    [Authorize(Roles = "Receptionist")]
    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateOffice([FromRoute] Guid id, [FromBody] UpdateOfficeDto updateOfficeDto)
    {
        await _officeService.UpdateOffice(id, updateOfficeDto);

        return NoContent();
    }

    [Authorize(Roles = "Receptionist")]
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteOffice([FromRoute] Guid id)
    {
        await _officeService.DeleteOffice(id);

        return NoContent();
    }
}
