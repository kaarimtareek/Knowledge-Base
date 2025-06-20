# Use the official .NET SDK image to build and publish the app
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["KnowledgeBase.Infrastructure/KnowledgeBase.Infrastructure.csproj", "KnowledgeBase.Infrastructure/"]
RUN dotnet restore "KnowledgeBase.Infrastructure/KnowledgeBase.Infrastructure.csproj"
COPY . .
WORKDIR "/src/KnowledgeBase.Infrastructure"
RUN dotnet build "KnowledgeBase.Infrastructure.csproj" -c Release -o /app/build
RUN dotnet publish "KnowledgeBase.Infrastructure.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Use the runtime image for running the app
FROM mcr.microsoft.com/dotnet/runtime:9.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "KnowledgeBase.Infrastructure.dll"]

