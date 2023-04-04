FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Service.One/Service.One.csproj", "Service.One/"]
RUN dotnet restore "Service.One/Service.One.csproj"
COPY . .
WORKDIR "/src/Service.One"
RUN dotnet build "Service.One.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Service.One.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Service.One.dll"]
