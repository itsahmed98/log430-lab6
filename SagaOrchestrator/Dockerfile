# Stage 1: build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# copy csproj and restore
COPY ["SagaOrchestrator.csproj", "./"]
RUN dotnet restore "./SagaOrchestrator.csproj"

# copy everything else and publish
COPY . .
RUN dotnet publish "SagaOrchestrator.csproj" -c Release -o /app/publish

# Stage 2: runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "SagaOrchestrator.dll"]