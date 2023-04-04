FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Service.Three/Service.Three.csproj", "Service.Three/"]
RUN dotnet restore "Service.Three/Service.Three.csproj"
COPY . .
WORKDIR "/src/Service.Three"
RUN dotnet build "Service.Three.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Service.Three.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Service.Three.dll"]
