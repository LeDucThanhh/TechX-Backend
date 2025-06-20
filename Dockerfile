FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["TechX.API.csproj", "."]
RUN dotnet restore "./TechX.API.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "TechX.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TechX.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TechX.API.dll"] 