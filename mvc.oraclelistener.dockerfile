FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["MVC.OracleListener/MVC.OracleListener.csproj", "MVC.OracleListener/"]
RUN dotnet restore "MVC.OracleListener/MVC.OracleListener.csproj"
COPY . .
WORKDIR "/src/MVC.OracleListener"
RUN dotnet build "MVC.OracleListener.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MVC.OracleListener.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MVC.OracleListener.dll"]
