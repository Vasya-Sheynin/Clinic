using MassTransit;
using Messages;
using ProfileRepositories;
using Profiles;

namespace Infrastructure.RabbitMq;

public class AccountCreatedMessageConsumer : IConsumer<AccountCreated>
{
    private readonly IUnitOfWork _unitOfWork;

    public AccountCreatedMessageConsumer(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Consume(ConsumeContext<AccountCreated> context)
    {
        await _unitOfWork.PatientProfileRepo.CreatePatientProfileAsync(new PatientProfile
        {
            Id = Guid.NewGuid(),
            AccountId = context.Message.AccountId
        });

        await _unitOfWork.SaveAsync();
    }
}