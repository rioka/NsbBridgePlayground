docker build -t nsbbridgeplayground-notifier -f .\NsbBridgePlayground.Notifier\Dockerfile .
docker build -t nsbbridgeplayground-shipping -f .\NsbBridgePlayground.Shipping\Dockerfile .
docker build -t nsbbridgeplayground-orderprocessor -f .\NsbBridgePlayground.OrderProcessor\Dockerfile .
docker build -t nsbbridgeplayground-sender -f .\NsbBridgePlayground.Sender\Dockerfile .
docker build -t nsbbridgeplayground-bridge -f .\NsbBridgePlayground.Bridge\Dockerfile .
