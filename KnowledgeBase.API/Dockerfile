FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app

ENV ASPNETCORE_URLS=http://0.0.0.0:6061

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["KnowledgeBase.API/KnowledgeBase.API.csproj", "KnowledgeBase.API/"]
RUN dotnet restore "KnowledgeBase.API/KnowledgeBase.API.csproj"
COPY . .
WORKDIR "/src/KnowledgeBase.API"
RUN dotnet build "./KnowledgeBase.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./KnowledgeBase.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "KnowledgeBase.API.dll"]
