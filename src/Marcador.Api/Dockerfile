# Imagen base con el SDK de .NET 8 para compilar
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar archivos de solución y proyectos para restaurar dependencias (aprovecha cache layers)
COPY ["Marcador.sln", "./"]
COPY ["src/Marcador.Api/Marcador.Api.csproj", "src/Marcador.Api/"]
COPY ["src/Marcador.Application/Marcador.Application.csproj", "src/Marcador.Application/"]
COPY ["src/Marcador.Domain/Marcador.Domain.csproj", "src/Marcador.Domain/"]
COPY ["src/Marcador.Infrastructure/Marcador.Infrastructure.csproj", "src/Marcador.Infrastructure/"]

# Restaurar dependencias
RUN dotnet restore "src/Marcador.Api/Marcador.Api.csproj"

# Copiar todo el código fuente
COPY . .

# Compilar y publicar en Release
WORKDIR "/src/src/Marcador.Api"
RUN dotnet publish "Marcador.Api.csproj" -c Release -o /app/publish --no-restore

# Imagen más ligera con ASP.NET Runtime para ejecutar
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

# Exponer el puerto 8080 (recomendado para contenedores)
EXPOSE 8080

# Configurar la aplicación para escuchar en el puerto 8080
ENV ASPNETCORE_URLS=http://+:8080

ENTRYPOINT ["dotnet", "Marcador.Api.dll"]