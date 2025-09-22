# Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar arquivos de projeto e restaurar dependências
COPY RiderGo.sln ./
COPY src/RiderGo.API/RiderGo.API.csproj src/RiderGo.API/
COPY src/RiderGo.Domain/RiderGo.Domain.csproj src/RiderGo.Domain/
COPY src/RiderGo.Infrastructure/RiderGo.Infrastructure.csproj src/RiderGo.Infrastructure/
RUN dotnet restore RiderGo.sln


COPY . .

# Publica a aplicação
WORKDIR "/src/src/RiderGo.API"
RUN dotnet publish "RiderGo.API.csproj" -c Release -o /app/publish


FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .


ENTRYPOINT ["dotnet", "RiderGo.API.dll"]