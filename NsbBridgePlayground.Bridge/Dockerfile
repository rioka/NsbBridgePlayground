﻿ARG CONFIGURATION=Release

FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
ARG CONFIGURATION
WORKDIR /src
COPY ["Directory.Build.props", "/"]
COPY ["NsbBridgePlayground.Bridge/NsbBridgePlayground.Bridge.csproj", "NsbBridgePlayground.Bridge/"]
COPY ["NsbBridgePlayground.Common/NsbBridgePlayground.Common.csproj", "NsbBridgePlayground.Common/"]
RUN dotnet restore "NsbBridgePlayground.Bridge/NsbBridgePlayground.Bridge.csproj"
COPY . .
WORKDIR "/src/NsbBridgePlayground.Bridge"
RUN dotnet build "NsbBridgePlayground.Bridge.csproj" -c $CONFIGURATION -o /app/build

FROM build AS publish
ARG CONFIGURATION
RUN dotnet publish "NsbBridgePlayground.Bridge.csproj" -c $CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NsbBridgePlayground.Bridge.dll"]
