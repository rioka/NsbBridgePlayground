## Components of the system

| Endpoint                             | Notes |
|--------------------------------------|-------|
| `NsbBridgePlayground.Sender`         |       |
| `NsbBridgePlayground.OrderProcessor` |       |
| `NsbBridgePlayground.Notifier`       |       |

## Behavior

```puml
@startuml

'[Sender] as S
'[OrderProcessor] as OP
'[Notifier] as N

'S --> OP : CreateOrder

Sender:

Sender --> OrderProcessor : ""CreateOrder""  

OrderProcessor -[dotted]> Notifier : ""OrderCreated""
```

Initially, the system does not depend on the bridge.

## Required Changes

- 💥Upgrade to NServiceBus v8

  > We should plan accordingly.
  >
  > This is required for the bridge only, at least initially: transports for v7 and v8 are compatible. 

- A new service, analogous to the router, using `NServiceBus.Transport.Bridge`

- Commands do not need any specific configuration: we can drop any router-dependent settings.

  > This means we have to use plain NServiceBus API, i.e. `RoutingSettings.RouteToEndpoint` for commands.  

- Endpoints need queues for each recipient: for example, in our case, `Sender` is sending commands to `OrderProcessor` so, instead of having a single queue (as it would be with the router), we have a queue for each endpoint the bridge is configured for.  

- For events, it's probably worse for the most part: as far as I can see, the bridge is responsible to send subscriptions explicitly: since endpoints are totally unaware of it, it is up to the bridge to send the service message to the publisher.

## References

- [Migrating from NServiceBus.Router to NServiceBus.Transport.Bridge](https://docs.particular.net/nservicebus/bridge/migrating-from-router)
