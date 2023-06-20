ARG CONFIGURATION=Release

FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
ARG CONFIGURATION
WORKDIR /src
COPY ["Directory.Build.props", "/"]
COPY ["NsbBridgePlayground.OrderProcessor/NsbBridgePlayground.OrderProcessor.csproj", "NsbBridgePlayground.OrderProcessor/"]
COPY ["NsbBridgePlayground.Common/NsbBridgePlayground.Common.csproj", "NsbBridgePlayground.Common/"]
COPY ["NsbBridgePlayground.Bootstrap/NsbBridgePlayground.Bootstrap.csproj", "NsbBridgePlayground.Bootstrap/"]
RUN dotnet restore "NsbBridgePlayground.OrderProcessor/NsbBridgePlayground.OrderProcessor.csproj"
COPY . .
WORKDIR "/src/NsbBridgePlayground.OrderProcessor"
RUN dotnet build "NsbBridgePlayground.OrderProcessor.csproj" -c $CONFIGURATION -o /app/build

FROM build AS publish
ARG CONFIGURATION
RUN dotnet publish "NsbBridgePlayground.OrderProcessor.csproj" -c $CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NsbBridgePlayground.OrderProcessor.dll"]
