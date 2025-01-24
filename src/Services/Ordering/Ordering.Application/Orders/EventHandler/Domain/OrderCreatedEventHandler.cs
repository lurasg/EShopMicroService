using MassTransit;
using Microsoft.FeatureManagement;

namespace Ordering.Application.Orders.EventHandler.Domain;

public class OrderCreatedEventHandler
    (IPublishEndpoint publisheEndpoint,IFeatureManager featureManager, ILogger<OrderCreatedEventHandler> logger)
    : INotificationHandler<OrderCreatedEvent>
{
    public async Task Handle(OrderCreatedEvent domainEvent, CancellationToken cancellationToken)
    {
        logger.LogInformation("Domain Event handled: {DomainEvent}", domainEvent.GetType().Name);

        if(await featureManager.IsEnabledAsync("OrderFullfilment"))
        {
            var orderCretaedIntegrationEvent = domainEvent.order.ToOrderDto();
            await publisheEndpoint.Publish(orderCretaedIntegrationEvent, cancellationToken);
        }
    }
}
