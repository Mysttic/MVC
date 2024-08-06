FROM mcr.microsoft.com/dotnet/runtime:8.0-windowsservercore-ltsc2022 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0-windowsservercore-ltsc2022 AS build
WORKDIR /src
COPY ["MVC.PostgreSQLListener/MVC.PostgreSQLListener.csproj", "MVC.PostgreSQLListener/"]
RUN dotnet restore "MVC.PostgreSQLListener/MVC.PostgreSQLListener.csproj"
COPY . .
WORKDIR "/src/MVC.PostgreSQLListener"
RUN dotnet build "MVC.PostgreSQLListener.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MVC.PostgreSQLListener.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MVC.PostgreSQLListener.dll"]
