using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ProfileRepositories;
using Profiles;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
using System.Threading.Channels;

namespace Persistence.RabbitMq;

public class MessageConsumer : BackgroundService
{
    private readonly IChannel _channel;
    private IConnection _connection;
    private readonly IUnitOfWork _unitOfWork;

    public MessageConsumer(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;

        var factory = new ConnectionFactory { HostName = "localhost" };
        _connection = factory.CreateConnectionAsync().Result;
        _channel = _connection.CreateChannelAsync().Result;

        _channel.QueueDeclareAsync(queue: "auth", durable: false, exclusive: false, autoDelete: false, arguments: null).Wait();
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var consumer = new AsyncEventingBasicConsumer(_channel);
        consumer.ReceivedAsync += async (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            var user = JsonSerializer.Deserialize<PatientProfile>(message);

            await _unitOfWork.PatientProfileRepo.CreatePatientProfileAsync(user);
            await _unitOfWork.SaveAsync();
        };

        await _channel.BasicConsumeAsync("auth", autoAck: true, consumer: consumer);
    }
}
