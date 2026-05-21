# --- Base Stage ---
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# --- Build Stage ---
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["cast.api/cast.api.csproj", "cast.api/"]
COPY ["cast.api/www/index.html", "cast.api/www/index.html"]
COPY ["cast.api.core/cast.api.core.csproj", "cast.api.core/"]
COPY ["cast.core/cast.core.csproj", "cast.core/"]

RUN dotnet restore "cast.api/cast.api.csproj"
COPY . .

WORKDIR "/src/cast.api"
RUN dotnet build "./cast.api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./cast.api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY ["cast.api/www/index.html", "www/index.html"]
ENTRYPOINT ["dotnet", "cast.api.dll"]