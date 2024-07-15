using Common.Contracts;
using MassTransit;
using Microsoft.Extensions.Logging;
using OrderService.Models;

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

    /// <summary>
    /// Used by MassTransit to track whether all dependencies have been fulfilled for composite event
    /// </summary>
    public int OrderFulfilledStatus { get; set; }

    /// <summary>
    /// Keep a copy of the initial request for future reference in the transaction
    /// </summary>
    public CreateOrderEvent Request {  get; set; }
}

public class OrderSateMachine : MassTransitStateMachine<OrderSagaState>
{
    // Saga state - resulted from previously occurred events
    public required State FulfillingOrder { get; set; }
    public required State DeliveringOrder { get; set; }

    // TODO: ideally notifications would be sent for each state change instead of as an explicit step
    public required State NotifyingCustomer { get; set; }
    public required State Completed { get; set; }

    // Saga events - may result in state changes
    public required Event<CreateOrderEvent> CreateOrder { get; set; }
    public required Event<FulfillDairyOrderEvent> FulfillOrder { get; set; }
    public required Event<DairyOrderFulfilledEvent> DairyOrderFulFilled { get; set; }
    public required Event<ProduceOrderFulfilledEvent> ProduceOrderFulFilled { get; set; }
    public required Event OrderFulfilled { get; set; }
    public required Event<DeliverOrderEvent> DeliverOrder { get; set; }
    public required Event<OrderDeliveredEvent> OrderDelivered { get; set; }
    public required Event<NotifyCustomerEvent> NotifyCustomer { get; set; }

    public OrderSateMachine()
    {
        Intialise();

        Initially(
            When(CreateOrder)
                .Then(ctx => ctx.Saga.Request = ctx.Message)
                // TODO: see if we can reduce this to one fufillment event
                .Publish(ctx => new FulfillDairyOrderEvent(ctx.Message.OrderId, ctx.Message.Items))
                .Publish(ctx => new FulfillProduceOrderEvent(ctx.Message.OrderId, ctx.Message.Items))
                .TransitionTo(FulfillingOrder));

        // An order is considered fulfilled when dairy and produce are fufilled
        CompositeEvent(() => OrderFulfilled, x => x.OrderFulfilledStatus, DairyOrderFulFilled, ProduceOrderFulFilled);

        During(FulfillingOrder,
            When(OrderFulfilled)
            .Publish(ctx => new DeliverOrderEvent(ctx.Saga.Request.OrderId, ctx.Saga.Request.Recipient))
            .TransitionTo(DeliveringOrder),
            
            Ignore(FulfillOrder)); // Ignore requests to fulfill an order that is already fulfilled

        During(DeliveringOrder,
            When(OrderDelivered)
            .Finalize(),

            Ignore(DeliverOrder)); // Ignore requests to deliver an order that is already being delivered
    }

    /// <summary>
    /// Initialises saga and state machine
    /// </summary>
    private void Intialise()
    {
        InstanceState(x => x.CurrentState);

        Event(() => CreateOrder, x => x.CorrelateById(context => context.Message.OrderId));
        Event(() => FulfillOrder, x => x.CorrelateById(context => context.Message.OrderId));
        Event(() => DairyOrderFulFilled, x => x.CorrelateById(context => context.Message.OrderId));
        Event(() => ProduceOrderFulFilled, x => x.CorrelateById(context => context.Message.OrderId));
        Event(() => DeliverOrder, x => x.CorrelateById(context => context.Message.OrderId));
        Event(() => OrderDelivered, x => x.CorrelateById(context => context.Message.OrderId));
        Event(() => NotifyCustomer, x => x.CorrelateById(context => context.Message.OrderId));
    }
}