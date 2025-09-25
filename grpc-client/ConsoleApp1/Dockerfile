# Multi-stage build for minimal console application image
# Use Alpine-based SDK for building
FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build
WORKDIR /src

# Copy project file first for Docker layer caching optimization
COPY ["grpc-client/ConsoleApp1/ConsoleApp1.csproj", "grpc-client/ConsoleApp1/"]
RUN dotnet restore "grpc-client/ConsoleApp1/ConsoleApp1.csproj"

# Copy all source files after restore
COPY . .
WORKDIR "/src/grpc-client/ConsoleApp1"

# Build the console application in Release mode
RUN dotnet build "ConsoleApp1.csproj" -c Release -o /app/build

# Publish stage - create optimized deployment
FROM build AS publish
RUN dotnet publish "ConsoleApp1.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage - use minimal Alpine-based .NET runtime
FROM mcr.microsoft.com/dotnet/runtime:8.0-alpine AS final
WORKDIR /app

# Install curl for health checks and server readiness
RUN apk add --no-cache curl

# Create non-root user for security
RUN adduser -D -s /bin/sh appuser
USER appuser

# Copy published application from build stage
COPY --from=publish /app/publish .

# Entry point to run the console client application
ENTRYPOINT ["dotnet", "ConsoleApp1.dll"]