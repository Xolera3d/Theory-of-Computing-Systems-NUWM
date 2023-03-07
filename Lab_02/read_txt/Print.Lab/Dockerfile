FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Print.Lab.csproj", "./"]
RUN dotnet restore "Print.Lab.csproj"
COPY . .
WORKDIR "/src/"
RUN dotnet build "Print.Lab.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Print.Lab.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Print.Lab.dll"]
