#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["OnlineAuctionBackend.API/OnlineAuctionBackend.API.csproj", "OnlineAuctionBackend.API/"]
RUN dotnet restore "OnlineAuctionBackend.API/OnlineAuctionBackend.API.csproj"
COPY . .
WORKDIR "/src/OnlineAuctionBackend.API"
RUN dotnet build "OnlineAuctionBackend.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "OnlineAuctionBackend.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
CMD ["dotnet", "OnlineAuctionBackend.API.dll"]