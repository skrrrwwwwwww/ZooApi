using MassTransit;
using Microsoft.Extensions.Logging;
using ZooApi.Application.Common.Contracts;

namespace ZooApi.Application.Services;

public class AnimalCreatedConsumer : IConsumer<AnimalCreated>
{
    private readonly ILogger<AnimalCreatedConsumer> _logger;

    public AnimalCreatedConsumer(ILogger<AnimalCreatedConsumer> logger)
    {
        _logger = logger;
    }

    public Task Consume(ConsumeContext<AnimalCreated> context)
    {
        var message = context.Message;
        
        _logger.LogInformation($"Получено новое животное: (ID: {message.Id}) Имя: {message.Name}, порода: {message.Species} ");

        return Task.CompletedTask;
    }
}