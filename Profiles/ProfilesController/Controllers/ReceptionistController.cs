using Application.Commands.ReceptionistCommands;
using Application.Dto.Receptionist;
using Application.Queries.ReceptionistQueries;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ProfilesController.Controllers;

[Route("profile/receptionist")]
[ApiController]
public class ReceptionistController : ControllerBase
{
    private readonly ISender _sender;

    public ReceptionistController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet]
    public async Task<ActionResult> GetReceptionistProfiles()
    {
        var profiles = await _sender.Send(new GetReceptionistProfilesQuery());

        return Ok(profiles);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> GetReceptionistProfileById([FromRoute] Guid id)
    {
        var profile = await _sender.Send(new GetReceptionistProfileByIdQuery(id));

        return Ok(profile);
    }

    [HttpPost]
    public async Task<ActionResult> CreateReceptionistProfile([FromBody] CreateReceptionistDto createReceptionistDto)
    {
        var profile = await _sender.Send(new CreateReceptionistCommand(createReceptionistDto));

        return CreatedAtAction(nameof(GetReceptionistProfileById), new { id = profile.Id }, profile);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateReceptionistProfile([FromRoute] Guid id, [FromBody] UpdateReceptionistDto updateReceptionistDto)
    {
        await _sender.Send(new UpdateReceptionistCommand(id, updateReceptionistDto));

        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteReceptionistProfile([FromRoute] Guid id)
    {
        await _sender.Send(new DeleteReceptionistCommand(id));

        return Ok();
    }
}
