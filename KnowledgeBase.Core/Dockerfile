# Use the official .NET SDK image to build and publish the app
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["KnowledgeBase.Core/KnowledgeBase.Core.csproj", "KnowledgeBase.Core/"]
RUN dotnet restore "KnowledgeBase.Core/KnowledgeBase.Core.csproj"
COPY . .
WORKDIR "/src/KnowledgeBase.Core"
RUN dotnet build "KnowledgeBase.Core.csproj" -c Release -o /app/build
RUN dotnet publish "KnowledgeBase.Core.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Use the runtime image for running the app
FROM mcr.microsoft.com/dotnet/runtime:9.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "KnowledgeBase.Core.dll"]

