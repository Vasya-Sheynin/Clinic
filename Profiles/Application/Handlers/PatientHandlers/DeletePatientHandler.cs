﻿using Application.Commands.PatientCommands;
using MediatR;
using ProfileRepositories;

namespace Application.Handlers.PatientHandlers;

internal class DeletePatientHandler : IRequestHandler<DeletePatientCommand>
{
    private readonly IPatientProfileRepo _repo;

    public DeletePatientHandler(IPatientProfileRepo repo)
    {
        _repo = repo;
    }

    public async Task Handle(DeletePatientCommand request, CancellationToken cancellationToken)
    {
        var profile = await _repo.GetPatientProfileAsync(request.Id);
        await _repo.DeletePatientProfileAsync(profile);
    }
}
