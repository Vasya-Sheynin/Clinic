using Application.Commands.PatientCommands;
using Application.Dto.Patient;
using Application.Filters;
using Application.Queries.PatientQueries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ProfilesController.Controllers;

[Route("profile/patient")]
[ApiController]
public class PatientController : ControllerBase
{
    private readonly ISender _sender;

    public PatientController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet]
    public async Task<ActionResult> GetPatientProfiles([FromQuery]PatientFilterParams filterParams)
    {
        var profiles = await _sender.Send(new GetPatientProfilesQuery(filterParams));

        return Ok(profiles);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> GetPatientProfileById([FromRoute]Guid id)
    {
        var profile = await _sender.Send(new GetPatientProfileByIdQuery(id));

        return Ok(profile);
    }

    [HttpPost]
    public async Task<ActionResult> CreatePatientProfile([FromBody]CreatePatientDto createPatientDto)
    {
        var profile = await _sender.Send(new CreatePatientCommand(createPatientDto));

        return CreatedAtAction(nameof(GetPatientProfileById), new { id = profile.Id }, profile);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdatePatientProfile([FromRoute]Guid id, [FromBody]UpdatePatientDto updatePatientDto)
    {
        await _sender.Send(new UpdatePatientCommand(id, updatePatientDto));

        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeletePatientProfile([FromRoute]Guid id)
    {
        await _sender.Send(new DeletePatientCommand(id));

        return Ok();
    }
}
