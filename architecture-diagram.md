# gRPC Demo Project Architecture

## Overview
This is a simple gRPC demonstration project consisting of a server and client application built with .NET 8.

## System Architecture

```
┌─────────────────────────────────────────────────────────────────┐
│                      gRPC Demo System                          │
└─────────────────────────────────────────────────────────────────┘

┌──────────────────────┐                    ┌──────────────────────┐
│   Console Client     │                    │    gRPC Server       │
│   (ConsoleApp1)      │◄──── gRPC ────────►│   (GrpcService1)     │
│                      │    HTTP/2 + SSL    │                      │
│  Port: N/A           │                    │  Port: 5117          │
└──────────────────────┘                    └──────────────────────┘
         │                                           │
         │                                           │
         ▼                                           ▼
┌──────────────────────┐                    ┌──────────────────────┐
│   Order Builder      │                    │  Greeter Service     │
│   Pattern Demo       │                    │  Implementation      │
└──────────────────────┘                    └──────────────────────┘
                                                     │
                                                     ▼
                                            ┌──────────────────────┐
                                            │  Proto Definition    │
                                            │   (greet.proto)      │
                                            └──────────────────────┘
```

## Component Details

### 1. gRPC Server (GrpcService1)
- **Framework**: ASP.NET Core 8.0
- **Port**: 5117 (HTTP)
- **Dependencies**:
  - Grpc.AspNetCore (v2.57.0)
- **Services**:
  - `GreeterService`: Implements the `Greeter` gRPC service
- **Configuration**: Program.cs:6-11

### 2. Console Client (ConsoleApp1)
- **Framework**: .NET Console App 8.0
- **Dependencies**:
  - Google.Protobuf (v3.32.0)
  - Grpc.Net.Client (v2.71.0)
  - Grpc.Tools (v2.72.0)
- **Features**:
  - Order/Address Builder Pattern Demo
  - gRPC Client Communication
- **Connection**: Connects to server at http://localhost:5117

### 3. Proto Contract (greet.proto)
- **Package**: `greet`
- **Namespace**: `GrpcService1`
- **Services**:
  - `Greeter` with `SayHello` RPC method
- **Messages**:
  - `HelloRequest` (name: string)
  - `HelloReply` (message: string)

### 4. Domain Models (Client-side)
- **Order**: Business entity with Id, Price, and Address
- **Address**: Shipping address with Street, City, State, Zip
- **Builder Pattern**: OrderBuilder and AddressBuilder for fluent object creation

## Communication Flow

```
1. Client creates gRPC channel to server (localhost:5117)
2. Client instantiates GreeterClient from generated proxy
3. Client calls SayHelloAsync with HelloRequest
4. Server GreeterService processes request
5. Server returns HelloReply with greeting message
6. Client displays response and demo data
```

## Technology Stack
- **.NET 8.0**: Runtime platform
- **gRPC**: Remote procedure call framework
- **Protocol Buffers**: Serialization format
- **ASP.NET Core**: Web hosting framework (server)
- **System.Text.Json**: JSON serialization (demo purposes)

## Project Structure
```
grpc-demo/
├── grpc-demo.sln
├── GrpcService1/                 # gRPC Server
│   ├── Program.cs               # Server startup
│   ├── Services/
│   │   └── GreeterService.cs    # Service implementation
│   ├── Protos/
│   │   └── greet.proto         # Service contract
│   └── GrpcService1.csproj     # Server project file
└── grpc-client/
    └── ConsoleApp1/             # gRPC Client
        ├── Program.cs           # Client application
        ├── Order.cs            # Domain models & builders
        ├── Protos/
        │   └── greet.proto     # Client contract (copy)
        └── ConsoleApp1.csproj  # Client project file
```