FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Service.Two/Service.Two.csproj", "Service.Two/"]
RUN dotnet restore "Service.Two/Service.Two.csproj"
COPY . .
WORKDIR "/src/Service.Two"
RUN dotnet build "Service.Two.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Service.Two.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Service.Two.dll"]
