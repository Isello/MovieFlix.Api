# Etapa de compilação (Build)
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copiar os ficheiros e restaurar dependências
COPY ["MovieFlix.Api.csproj", "./"]
RUN dotnet restore "./MovieFlix.Api.csproj"

# Copiar o resto do código e compilar
COPY . .
RUN dotnet publish "MovieFlix.Api.csproj" -c Release -o /app/publish

# Etapa de execução (Runtime)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
EXPOSE 8080

# Copiar os ficheiros compilados da etapa anterior
COPY --from=build /app/publish .

# Definir o comando de arranque
ENTRYPOINT ["dotnet", "MovieFlix.Api.dll"]