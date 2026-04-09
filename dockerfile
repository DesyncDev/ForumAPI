FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copia os arquivos
COPY src/Forum.Api/Forum.Api.csproj src/Forum.Api/
COPY src/Forum.Domain/Forum.Domain.csproj src/Forum.Domain/
COPY src/Forum.Application/Forum.Application.csproj src/Forum.Application/
COPY src/Forum.Infrastructure/Forum.Infrastructure.csproj src/Forum.Infrastructure/

# Restaura as dependências
RUN dotnet restore "src/Forum.Api/Forum.Api.csproj"

# Copia o código fonte
COPY . .

# Publica a aplicação
WORKDIR "/src/src/Forum.Api"
RUN dotnet publish "Forum.Api.csproj" \
    -c Release \
    -o /app/publish \
    --no-restore

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app

# Copia o build publicado
COPY --from=build /app/publish .

# Portas
EXPOSE 8080
EXPOSE 8081

# Entrypoint
ENTRYPOINT ["dotnet", "Forum.Api.dll"]