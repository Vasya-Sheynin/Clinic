using Application.Commands.DoctorCommands;
using Application.Dto.Doctor;
using Application.Queries.DoctorQueries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProfileRepositories;
using ProfileRepositories.Pagination;

namespace ProfilesController.Controllers;

[Route("api/profile/doctor")]
[ApiController]
public class DoctorController : ControllerBase
{
    private readonly ISender _sender;

	public DoctorController(ISender sender)
	{
		_sender = sender;
	}

	[Authorize(Roles = "Patient, Receptionist")]
	[HttpGet]
	public async Task<ActionResult> GetDoctorProfiles([FromQuery]DoctorFilter filterParams, [FromQuery] PaginationParams paginationParams)
	{
		var profiles = await _sender.Send(new GetDoctorProfilesQuery(filterParams, paginationParams));

		return Ok(profiles);
	}

	[Authorize(Roles = "Patient, Receptionist")]
	[HttpGet("{id}")]
	public async Task<ActionResult> GetDoctorProfileById([FromRoute]Guid id)
	{
		var profile = await _sender.Send(new GetDoctorProfileByIdQuery(id));

		return Ok(profile);
	}

	[Authorize(Roles = "Receptionist")]
	[HttpPost]
	public async Task<ActionResult> CreateDoctorProfile([FromBody]CreateDoctorDto createDoctorDto)
	{
		var profile = await _sender.Send(new CreateDoctorCommand(createDoctorDto));

        return CreatedAtAction(nameof(GetDoctorProfileById), new { id = profile.Id }, profile);
    }

	[Authorize(Roles = "Doctor, Receptionist")]
	[HttpPut("{id}")]
	public async Task<ActionResult> UpdateDoctorProfile([FromRoute]Guid id, [FromBody]UpdateDoctorDto updateDoctorDto)
	{
		await _sender.Send(new UpdateDoctorCommand(id, updateDoctorDto));

		return Ok();
	}

	[Authorize(Roles = "Receptionist")]
	[HttpDelete("{id}")]
	public async Task<ActionResult> DeleteDoctorProfile([FromRoute]Guid id)
	{
        await _sender.Send(new DeleteDoctorCommand(id));

        return Ok();
    }
}
