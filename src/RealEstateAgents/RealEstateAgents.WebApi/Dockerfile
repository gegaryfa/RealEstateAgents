#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["RealEstateAgents/RealEstateAgents.WebApi/RealEstateAgents.WebApi.csproj", "RealEstateAgents/RealEstateAgents.WebApi/"]
COPY ["RealEstateAgents/RealEstateAgents.Infrastructure.Shared/RealEstateAgents.Infrastructure.Shared.csproj", "RealEstateAgents/RealEstateAgents.Infrastructure.Shared/"]
COPY ["RealEstateAgents/RealEstateAgents.Application/RealEstateAgents.Application.csproj", "RealEstateAgents/RealEstateAgents.Application/"]
COPY ["RealEstateAgents/RealEstateAgents.Domain/RealEstateAgents.Domain.csproj", "RealEstateAgents/RealEstateAgents.Domain/"]
RUN dotnet restore "RealEstateAgents/RealEstateAgents.WebApi/RealEstateAgents.WebApi.csproj"
COPY . .
WORKDIR "/src/RealEstateAgents/RealEstateAgents.WebApi"
RUN dotnet build "RealEstateAgents.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "RealEstateAgents.WebApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RealEstateAgents.WebApi.dll"]
