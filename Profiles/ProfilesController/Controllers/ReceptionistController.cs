using Application.Commands.ReceptionistCommands;
using Application.Dto.Receptionist;
using Application.Queries.ReceptionistQueries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProfileRepositories.Pagination;

namespace ProfilesController.Controllers;

[Route("api/profile/receptionist")]
[ApiController]
public class ReceptionistController : ControllerBase
{
    private readonly ISender _sender;

    public ReceptionistController(ISender sender)
    {
        _sender = sender;
    }

    [Authorize(Roles = "Receptionist")]
    [HttpGet]
    public async Task<ActionResult> GetReceptionistProfiles([FromQuery]PaginationParams paginationParams)
    {
        var profiles = await _sender.Send(new GetReceptionistProfilesQuery(paginationParams));

        return Ok(profiles);
    }

    [Authorize(Roles = "Receptionist")]
    [HttpGet("{id}")]
    public async Task<ActionResult> GetReceptionistProfileById([FromRoute] Guid id)
    {
        var profile = await _sender.Send(new GetReceptionistProfileByIdQuery(id));

        return Ok(profile);
    }

    [Authorize(Roles = "Receptionist")]
    [HttpPost]
    public async Task<ActionResult> CreateReceptionistProfile([FromBody] CreateReceptionistDto createReceptionistDto)
    {
        var profile = await _sender.Send(new CreateReceptionistCommand(createReceptionistDto));

        return CreatedAtAction(nameof(GetReceptionistProfileById), new { id = profile.Id }, profile);
    }

    [Authorize(Roles = "Receptionist")]
    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateReceptionistProfile([FromRoute] Guid id, [FromBody] UpdateReceptionistDto updateReceptionistDto)
    {
        await _sender.Send(new UpdateReceptionistCommand(id, updateReceptionistDto));

        return Ok();
    }

    [Authorize(Roles = "Receptionist")]
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteReceptionistProfile([FromRoute] Guid id)
    {
        await _sender.Send(new DeleteReceptionistCommand(id));

        return Ok();
    }
}
