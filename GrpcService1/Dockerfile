# Multi-stage build for minimal image size
# Use Alpine-based SDK for building (smaller base image)
FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build
WORKDIR /src

# Copy project file first for better layer caching
# This allows Docker to cache the restore step when source code changes
COPY ["GrpcService1/GrpcService1.csproj", "GrpcService1/"]
RUN dotnet restore "GrpcService1/GrpcService1.csproj"

# Copy all source files after restore
COPY . .
WORKDIR "/src/GrpcService1"

# Build the application in Release mode for optimization
RUN dotnet build "GrpcService1.csproj" -c Release -o /app/build

# Publish stage - create self-contained deployment
FROM build AS publish
RUN dotnet publish "GrpcService1.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage - use minimal Alpine-based ASP.NET runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS final
WORKDIR /app

# Create non-root user for security
RUN adduser -D -s /bin/sh appuser
USER appuser

# Copy published application from build stage
COPY --from=publish /app/publish .

# Expose port for gRPC service (HTTP/2)
EXPOSE 8080

# Configure ASP.NET Core to bind to all interfaces and port 8080
ENV ASPNETCORE_URLS=http://+:8080

# Start the gRPC service
ENTRYPOINT ["dotnet", "GrpcService1.dll"]