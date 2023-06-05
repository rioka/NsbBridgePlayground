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

- Commands do not need any specific configuration: we can drop any router-dependent settings. 

## References

- [Migrating from NServiceBus.Router to NServiceBus.Transport.Bridge](https://docs.particular.net/nservicebus/bridge/migrating-from-router)

