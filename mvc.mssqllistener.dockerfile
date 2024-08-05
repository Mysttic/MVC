FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["MVC.MSSQLListener/MVC.MSSQLListener.csproj", "MVC.MSSQLListener/"]
RUN dotnet restore "MVC.MSSQLListener/MVC.MSSQLListener.csproj"
COPY . .
WORKDIR "/src/MVC.MSSQLListener"
RUN dotnet build "MVC.MSSQLListener.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MVC.MSSQLListener.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MVC.MSSQLListener.dll"]
