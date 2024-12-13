using MediatR;

namespace Application.Commands;

public record DeleteDoctorCommand(Guid Id) : IRequest;