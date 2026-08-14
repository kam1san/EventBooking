FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["EventBooking.Worker/EventBooking.Worker.csproj", "EventBooking.Worker/"]
COPY ["Domain/Domain.csproj", "Domain/"]
RUN dotnet restore "./EventBooking.Worker/EventBooking.Worker.csproj"
COPY . .
WORKDIR "/src/EventBooking.Worker"
RUN dotnet build "./EventBooking.Worker.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./EventBooking.Worker.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EventBooking.Worker.dll"]