FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY . .
WORKDIR /src/SmartQueue.Api

RUN dotnet restore "SmartQueue.Api.csproj"
RUN dotnet publish "SmartQueue.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app

COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:5012
EXPOSE 50121

ENTRYPOINT ["dotnet", "SmartQueue.Api.dll"]
