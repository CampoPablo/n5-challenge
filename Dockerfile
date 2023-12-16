FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app


# Creates a non-root user with an explicit UID and adds permission to access the /app folder
# For more info, please refer to https://aka.ms/vscode-docker-dotnet-configure-containers
RUN adduser -u 5678 --disabled-password --gecos "" appuser && chown -R appuser /app
USER appuser

FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:7.0 AS build
ARG configuration=Release
WORKDIR /src

COPY ["ChallengeN5Solution.sln", "./"]
COPY ["n5.Infrastructure/n5.Infrastructure.csproj", "n5.Infrastructure/"]
COPY ["n5.Application/n5.Application.csproj", "n5.Application/"]
COPY ["n5.WebApi/n5.WebApi.csproj", "n5.WebApi/"]

RUN dotnet restore "n5.WebApi/n5.WebApi.csproj"
# RUN dotnet restore 
COPY . .
WORKDIR "/src/n5.WebApi"
RUN dotnet build "n5.WebApi.csproj" -c $configuration -o /app/build

FROM build AS publish
ARG configuration=Release
RUN dotnet publish "n5.WebApi.csproj" -c $configuration -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "n5.WebApi.dll"]
