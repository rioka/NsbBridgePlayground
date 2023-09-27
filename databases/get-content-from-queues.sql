/* sender queues */
SELECT 'Sender queue on Sender'
     ,[Id]
     ,[CorrelationId]
     ,[ReplyToAddress]
     ,[Recoverable]
     ,[Expires]
     ,[Headers]
     ,[Body]
     ,[RowVersion]
FROM
    [NsbBridgePlayground.Sender].[nsb].[Sender];

SELECT 'OrderProcessor queue on Sender'
     ,[Id]
     ,[CorrelationId]
     ,[ReplyToAddress]
     ,[Recoverable]
     ,[Expires]
     ,[Headers]
     ,[Body]
     ,[RowVersion]
FROM
    [NsbBridgePlayground.Sender].[nsb].[OrderProcessor];

SELECT 'Notifier queue on Sender'
     ,[Id]
     ,[CorrelationId]
     ,[ReplyToAddress]
     ,[Recoverable]
     ,[Expires]
     ,[Headers]
     ,[Body]
     ,[RowVersion]
FROM
    [NsbBridgePlayground.Sender].[nsb].[Notifier];

SELECT 'Shipping queue on Sender'
     ,[Id]
     ,[CorrelationId]
     ,[ReplyToAddress]
     ,[Recoverable]
     ,[Expires]
     ,[Headers]
     ,[Body]
     ,[RowVersion]
FROM
    [NsbBridgePlayground.Sender].[nsb].[Shipping];

SELECT 'SubscriptionRouting on Sender'
     ,*
FROM
    [NsbBridgePlayground.Sender].[nsb].[SubscriptionRouting];

/* OrderProcessor queues */
SELECT 'OrderProcessor queue on OrderProcessor'
     ,[Id]
     ,[CorrelationId]
     ,[ReplyToAddress]
     ,[Recoverable]
     ,[Expires]
     ,[Headers]
     ,[Body]
     ,[RowVersion]
FROM
    [NsbBridgePlayground.OrderProcessor].[nsb].[OrderProcessor];

SELECT 'Sender queue on OrderProcessor'
     ,[Id]
     ,[CorrelationId]
     ,[ReplyToAddress]
     ,[Recoverable]
     ,[Expires]
     ,[Headers]
     ,[Body]
     ,[RowVersion]
FROM
    [NsbBridgePlayground.OrderProcessor].[nsb].[Sender];

SELECT 'Notifier queue on OrderProcessor'
     ,[Id]
     ,[CorrelationId]
     ,[ReplyToAddress]
     ,[Recoverable]
     ,[Expires]
     ,[Headers]
     ,[Body]
     ,[RowVersion]
FROM
    [NsbBridgePlayground.OrderProcessor].[nsb].[Notifier];

SELECT 'Shipping queue on OrderProcessor'
     ,[Id]
     ,[CorrelationId]
     ,[ReplyToAddress]
     ,[Recoverable]
     ,[Expires]
     ,[Headers]
     ,[Body]
     ,[RowVersion]
FROM
    [NsbBridgePlayground.OrderProcessor].[nsb].[Shipping];

SELECT 'SubscriptionRouting on Receiver'
     ,*
FROM
    [NsbBridgePlayground.OrderProcessor].[nsb].[SubscriptionRouting];

/* notifier queues */
SELECT 'Notifier queue on Notifier'
     ,[Id]
     ,[CorrelationId]
     ,[ReplyToAddress]
     ,[Recoverable]
     ,[Expires]
     ,[Headers]
     ,[Body]
     ,[RowVersion]
FROM
    [NsbBridgePlayground.Notifier].[nsb].[Notifier];

SELECT 'Sender queue on Notifier'
     ,[Id]
     ,[CorrelationId]
     ,[ReplyToAddress]
     ,[Recoverable]
     ,[Expires]
     ,[Headers]
     ,[Body]
     ,[RowVersion]
FROM
    [NsbBridgePlayground.Notifier].[nsb].[Sender];

SELECT 'OrderProcessor queue on Notifier'
     ,[Id]
     ,[CorrelationId]
     ,[ReplyToAddress]
     ,[Recoverable]
     ,[Expires]
     ,[Headers]
     ,[Body]
     ,[RowVersion]
FROM
    [NsbBridgePlayground.Notifier].[nsb].[OrderProcessor];

SELECT 'Shipping queue on Notifier'
     ,[Id]
     ,[CorrelationId]
     ,[ReplyToAddress]
     ,[Recoverable]
     ,[Expires]
     ,[Headers]
     ,[Body]
     ,[RowVersion]
FROM
    [NsbBridgePlayground.Notifier].[nsb].[Shipping];

SELECT 'SubscriptionRouting on Notifier'
     ,*
FROM
    [NsbBridgePlayground.Notifier].[nsb].[SubscriptionRouting];

/* shipping queues */
SELECT 'Shipping queue on Shipping'
     ,[Id]
     ,[CorrelationId]
     ,[ReplyToAddress]
     ,[Recoverable]
     ,[Expires]
     ,[Headers]
     ,[Body]
     ,[RowVersion]
FROM
    [NsbBridgePlayground.Shipping].[nsb].[Shipping];

SELECT 'Sender queue on Shipping'
     ,[Id]
     ,[CorrelationId]
     ,[ReplyToAddress]
     ,[Recoverable]
     ,[Expires]
     ,[Headers]
     ,[Body]
     ,[RowVersion]
FROM
    [NsbBridgePlayground.Shipping].[nsb].[Sender];

SELECT 'OrderProcessor queue on Shipping'
     ,[Id]
     ,[CorrelationId]
     ,[ReplyToAddress]
     ,[Recoverable]
     ,[Expires]
     ,[Headers]
     ,[Body]
     ,[RowVersion]
FROM
    [NsbBridgePlayground.Shipping].[nsb].[OrderProcessor];

SELECT 'Notifier queue on Shipping'
     ,[Id]
     ,[CorrelationId]
     ,[ReplyToAddress]
     ,[Recoverable]
     ,[Expires]
     ,[Headers]
     ,[Body]
     ,[RowVersion]
FROM
    [NsbBridgePlayground.Shipping].[nsb].[Notifier];

SELECT 'SubscriptionRouting on Shipping'
     ,*
FROM
    [NsbBridgePlayground.Shipping].[nsb].[SubscriptionRouting];
