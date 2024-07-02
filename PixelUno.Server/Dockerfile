FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["PixelUno.Server/PixelUno.Server.csproj", "PixelUno.Server/"]
COPY ["PixelUno.Shared/PixelUno.Shared.csproj", "PixelUno.Shared/"]
RUN dotnet restore "PixelUno.Server/PixelUno.Server.csproj"
COPY . .
WORKDIR "/src/PixelUno.Server"
RUN dotnet build "PixelUno.Server.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "PixelUno.Server.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PixelUno.Server.dll"]
