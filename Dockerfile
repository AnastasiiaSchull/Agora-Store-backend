# Базовый образ для runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base

# Аргумент для UID пользователя, можно задавать при сборке
ARG APP_UID=1654

WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Создаём группу и пользователя с заданным UID/GID
RUN addgroup -g $APP_UID appgroup && \
    adduser -u $APP_UID -G appgroup -S app

# Меняем владельца и права на папку wwwroot
RUN mkdir -p /app/wwwroot && \
    chown -R app:appgroup /app/wwwroot && \
    chmod -R u+rwX /app/wwwroot

# Переключаемся на пользователя app
USER app

# Этап сборки
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Agora.WebApi/Agora.csproj", "Agora.WebApi/"]
RUN dotnet restore "./Agora.WebApi/Agora.csproj"
COPY . .
WORKDIR "/src/Agora.WebApi"
RUN dotnet build "./Agora.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Этап публикации
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Agora.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Финальный этап
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "Agora.dll"]

