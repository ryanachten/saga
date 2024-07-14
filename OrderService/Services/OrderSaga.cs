using Common.Contracts;
using MassTransit;

namespace OrderService.Services;

public class OrderSagaState : SagaStateMachineInstance
{
    /// <summary>
    /// Uniquely identifies a transaction comprised of multiple events
    /// </summary>
    public Guid CorrelationId { get; set; }

    /// <summary>
    /// Name of the current saga state
    /// </summary>
    public required string CurrentState { get; set; }

    public bool IsDairyFulfilled { get; set; }
    public bool IsProduceFulfilled { get; set; }
}

public class OrderSateMachine : MassTransitStateMachine<OrderSagaState>
{
    // Saga state - resulted from previously occurred events
    public required State FulfillingOrder { get; set; }
    public required State Fulfilled { get; set; }
    public required State Delivered { get; set; }
    public required State Completed { get; set; }

    // Saga events - may result in state changes
    public required Event<CreateOrderEvent> CreateOrder { get; set; }
    public required Event<FulfillOrderEvent> FulFillOrder { get; set; }
    public required Event<DeliverOrderEvent> DeliverOrder { get; set; }
    public required Event<NotifyCustomerEvent> NotifyCustomer { get; set; }

    public OrderSateMachine()
    {
        Intialise();

        Initially(
            When(CreateOrder)
                .Publish(ctx => new FulfillOrderEvent(ctx.Message.OrderId, ctx.Message.Items))
                .TransitionTo(FulfillingOrder)
        );
    }

    /// <summary>
    /// Initialises saga and state machine
    /// </summary>
    private void Intialise()
    {
        InstanceState(x => x.CurrentState);

        Event(() => CreateOrder, x => x.CorrelateById(context => context.Message.OrderId));
        Event(() => FulFillOrder, x => x.CorrelateById(context => context.Message.OrderId));
        Event(() => DeliverOrder, x => x.CorrelateById(context => context.Message.OrderId));
        Event(() => NotifyCustomer, x => x.CorrelateById(context => context.Message.OrderId));
    }
}