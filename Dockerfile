# Application builder
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS builder

WORKDIR /app

COPY . .

RUN dotnet restore

RUN dotnet publish -c Release -o out

# Runtime image
FROM mcr.microsoft.com/dotnet/aspnet:6.0

WORKDIR /app

COPY --from=builder /app/out .

ENTRYPOINT ["dotnet", "PixelWorldsServer.Server.dll"]
