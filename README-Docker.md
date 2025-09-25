# Docker Setup for gRPC Demo

## Quick Start

Build and run both services:
```bash
docker-compose up --build
```

## Commands

### Development
```bash
# Build and run with logs
docker-compose up --build

# Run in background
docker-compose up -d --build

# View logs
docker-compose logs -f

# Stop services
docker-compose down
```

### Individual Services
```bash
# Build server only
docker build -f GrpcService1/Dockerfile -t grpc-server .

# Build client only
docker build -f grpc-client/ConsoleApp1/Dockerfile -t grpc-client .

# Run server standalone
docker run -p 8080:8080 grpc-server

# Run client (connect to host server)
docker run --network host grpc-client
```

## Image Sizes
- Server: ~180MB (Alpine-based)
- Client: ~90MB (Alpine-based)

## Architecture
- **Network**: Custom bridge network for service discovery
- **Health Checks**: Server readiness validation
- **Security**: Non-root users in containers
- **Resource Limits**: Memory and CPU constraints