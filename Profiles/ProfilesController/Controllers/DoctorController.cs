﻿using Application.Commands;
using Application.Dto.Doctor;
using Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ProfilesController.Controllers;

[Route("profile")]
[ApiController]
public class DoctorController : ControllerBase
{
    private readonly ISender _sender;

	public DoctorController(ISender sender)
	{
		_sender = sender;
	}

	[HttpGet]
	public async Task<ActionResult> GetDoctorProfiles()
	{
		var profiles = await _sender.Send(new GetDoctorProfilesQuery());

		return Ok(profiles);
	}

	[HttpGet("{id}")]
	public async Task<ActionResult> GetDoctorProfileById([FromRoute]Guid id)
	{
		var profile = await _sender.Send(new GetDoctorProfileByIdQuery(id));

		return Ok(profile);
	}

	[HttpPost]
	public async Task<ActionResult> CreateDoctorProfile([FromBody]CreateDoctorDto createDoctorDto)
	{
		var profile = await _sender.Send(new CreateDoctorCommand(createDoctorDto));

        return CreatedAtAction(nameof(GetDoctorProfileById), new { id = profile.Id }, profile);
    }

	[HttpPut("{id}")]
	public async Task<ActionResult> UpdateDoctorProfile([FromRoute]Guid id, [FromBody]UpdateDoctorDto updateDoctorDto)
	{
		await _sender.Send(new UpdateDoctorCommand(id, updateDoctorDto));

		return Ok();
	}

	[HttpDelete("{id}")]
	public async Task<ActionResult> DeleteDoctorProfile([FromRoute]Guid id)
	{
        await _sender.Send(new DeleteDoctorCommand(id));

        return Ok();
    }
}
