#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["src/Services/Basket/Basket.Api/Basket.Api.csproj", "src/Services/Basket/Basket.Api/"]
COPY ["src/BuildingBlocks/EventBus.Message/EventBus.Message.csproj", "src/BuildingBlocks/EventBus.Message/"]
RUN dotnet restore "src/Services/Basket/Basket.Api/Basket.Api.csproj"
COPY . .
WORKDIR "/src/src/Services/Basket/Basket.Api"
RUN dotnet build "Basket.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Basket.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Basket.Api.dll"]