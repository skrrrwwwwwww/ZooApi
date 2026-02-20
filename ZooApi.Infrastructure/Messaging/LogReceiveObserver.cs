using Microsoft.Extensions.Logging;

namespace ZooApi.Infrastructure.Messaging;

public class LogReceiveObserver(ILogger<LogReceiveObserver> logger) : IReceiveObserver
{
    public Task ConsumeFault<T>(ConsumeContext<T> context, 
                                TimeSpan duration, 
                                string consumerType, 
                                Exception exception) where T : class
    {
        logger.LogError(exception, "Ошибка в консьюмере {Consumer}: {MessageId}. Сообщение: {ExceptionMessage}", 
            consumerType, context.MessageId, exception.Message);
        return Task.CompletedTask;
    }

    public Task ReceiveFault(ReceiveContext context, Exception exception) => Task.CompletedTask;
    public Task PreReceive(ReceiveContext context) => Task.CompletedTask;
    public Task PostReceive(ReceiveContext context) => Task.CompletedTask;
    public Task PostConsume<T>(ConsumeContext<T> context, 
                                    TimeSpan duration, 
                                    string consumerType) 
                                    where T : class 
        => Task.CompletedTask;
}