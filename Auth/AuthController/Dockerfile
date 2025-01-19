# См. статью по ссылке https://aka.ms/customizecontainer, чтобы узнать как настроить контейнер отладки и как Visual Studio использует этот Dockerfile для создания образов для ускорения отладки.

# Этот этап используется при запуске из VS в быстром режиме (по умолчанию для конфигурации отладки)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# Этот этап используется для сборки проекта службы
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Auth/AuthController/AuthController.csproj", "Auth/AuthController/"]
COPY ["Auth/Infrastructure/Infrastructure.csproj", "Auth/Infrastructure/"]
COPY ["Messages/Messages.csproj", "Messages/"]
COPY ["Auth/Application/Application.csproj", "Auth/Application/"]
COPY ["Auth/Domain/Persistence/Persistence.csproj", "Auth/Domain/Persistence/"]
COPY ["Auth/Domain/User/User.csproj", "Auth/Domain/User/"]
RUN dotnet restore "./Auth/AuthController/AuthController.csproj"
COPY . .
WORKDIR "/src/Auth/AuthController"
RUN dotnet build "./AuthController.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Этот этап используется для публикации проекта службы, который будет скопирован на последний этап
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./AuthController.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Этот этап используется в рабочей среде или при запуске из VS в обычном режиме (по умолчанию, когда конфигурация отладки не используется)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AuthController.dll"]