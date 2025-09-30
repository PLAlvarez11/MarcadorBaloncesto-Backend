# Etapa de build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar sln y csproj para restaurar dependencias
COPY ["Marcador.sln", "./"]
COPY ["src/Marcador.Api/Marcador.Api.csproj", "src/Marcador.Api/"]
COPY ["src/Marcador.Application/Marcador.Application.csproj", "src/Marcador.Application/"]
COPY ["src/Marcador.Domain/Marcador.Domain.csproj", "src/Marcador.Domain/"]
COPY ["src/Marcador.Infrastructure/Marcador.Infrastructure.csproj", "src/Marcador.Infrastructure/"]

RUN dotnet restore "src/Marcador.Api/Marcador.Api.csproj"

# Copiar el resto del código
COPY . .

# Publicar en Release
WORKDIR "/src/src/Marcador.Api"
RUN dotnet publish "Marcador.Api.csproj" -c Release -o /app/publish --no-restore

# Etapa de runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Marcador.Api.dll"]
