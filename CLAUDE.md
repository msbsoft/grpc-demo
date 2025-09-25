# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

# IMPORTANT
- Always prioritize writing clean, simple and modular code.
- Use simple & easy to understand language. Write in short sentences.
- DO NOT BE LAZY! Always read files in FULL!!

# COMMENTS
- Write lots of comments in your code. explain exactly what you are doing in your comments.
- Be strategic, do not explain obvious syntax instad explain your though process at the time of writing code.
- NEVER delete explanatory comments from code you're editing (unless they are wrong/obsolete).
  
# Code Style (.NET/C#)
- Follow Microsoft C# coding conventions and naming guidelines
- Use PascalCase for public members, methods, and properties
- Use camelCase for private fields and local variables
- Use meaningful, descriptive names for classes, methods, and variables
- Organize using statements alphabetically
- Use file-scoped namespaces (namespace GrpcService1.Services;)
- Prefer explicit type declarations when type is not obvious
- Use var only when the type is clear from the right side of assignment
  
## Common Development Commands

### Building the Solution
```bash
dotnet build
```

### Running the Server
```bash
dotnet run --project GrpcService1
```
Server will start on http://localhost:5117

### Running the Client
```bash
dotnet run --project grpc-client/ConsoleApp1
```

### Running Tests
```bash
# Run all tests
dotnet test

# Run specific test project
dotnet test GrpcService1.Tests
dotnet test ConsoleApp1.Tests

# Run with coverage
dotnet test --collect:"XPlat Code Coverage"
```

### Restore Dependencies
```bash
dotnet restore
```

## Project Architecture

This is a gRPC demonstration project built with .NET 8 consisting of:

1. **GrpcService1** - ASP.NET Core gRPC server that implements the Greeter service
2. **ConsoleApp1** - Console client application that connects to the gRPC server and demonstrates builder patterns
3. **Test Projects** - xUnit test projects for both server and client components

### Key Components

- **Protocol Buffer Contract**: `Protos/greet.proto` defines the gRPC service interface with `SayHello` RPC method
- **Server Implementation**: `Services/GreeterService.cs` implements the gRPC service
- **Client Models**: `Order.cs` contains domain models (Order, Address) with builder pattern implementations
- **Generated Code**: Protocol buffer compiler generates client/server stubs in `obj/Debug/net8.0/Protos/`

### Communication Flow
- Client connects to server at localhost:5117
- Uses gRPC over HTTP/2 for RPC calls
- Both projects reference the same proto contract

### Testing Framework
- **xUnit** for unit testing
- **Microsoft.AspNetCore.Mvc.Testing** for integration tests
- **Moq** for mocking dependencies
- Tests cover both domain logic (builders) and gRPC service functionality

## CI/CD Pipeline

### GitHub Actions Workflows

The project includes a comprehensive CI/CD pipeline with the following workflows:

#### Main CI/CD Pipeline (`ci-cd.yml`)
```bash
# Triggered on push to main/develop/feature branches and PRs
# Runs on: Ubuntu, Windows, macOS
```

**Pipeline Stages:**
1. **Build & Test**: Cross-platform build and comprehensive testing
2. **Security Scan**: Automated security vulnerability scanning
3. **Quality Gate**: Enforces quality standards and coverage thresholds
4. **Notifications**: Success/failure notifications

**Test Execution Strategy:**
- **Unit Tests**: Tests domain logic (builders) and service implementations
- **Integration Tests**: End-to-end gRPC service testing with TestServer
- **Regression Tests**: Full test suite execution
- **Code Coverage**: Minimum 80% threshold enforced

#### Auto PR Creation (`auto-pr.yml`)
```bash
# Automatically creates PRs after successful CI runs
# Triggers on successful completion of main CI/CD workflow
```

**Features:**
- Auto-creates PRs for feature/bugfix/hotfix branches
- Adds appropriate labels based on branch naming
- Includes detailed test results and quality gate status
- Updates existing PRs with latest CI results

### Quality Gates

All the following criteria must pass:
- ✅ Unit tests (100% pass rate)
- ✅ Integration tests (100% pass rate)
- ✅ Code coverage ≥80%
- ✅ Security scan (no vulnerabilities)
- ✅ Multi-platform build success

### Branch Strategy

- `main/master`: Production-ready code
- `develop`: Development integration branch
- `feature/*`: New features
- `bugfix/*`: Bug fixes
- `hotfix/*`: Critical fixes

### Dependency Management

**Dependabot** automatically:
- Updates NuGet packages weekly
- Groups related dependencies
- Creates PRs for security updates
- Manages GitHub Actions updates

### CI/CD Commands

```bash
# View workflow status
gh workflow list

# Trigger manual workflow run
gh workflow run ci-cd.yml

# View recent runs
gh run list --workflow=ci-cd.yml

# Download artifacts
gh run download [run-id]
```

# .NET Best Practices

## Project Structure and Organization
- Keep related classes in the same namespace
- Separate concerns: services in `/Services`, models in `/Models`, etc.
- Use meaningful folder structure that reflects the application architecture
- Keep .proto files in `/Protos` folder and sync between client/server

## Dependency Injection
- Register services in Program.cs using the built-in DI container
- Use constructor injection for dependencies
- Prefer interfaces over concrete implementations for testability
- Example: `builder.Services.AddGrpc();` in Program.cs:6

## Async/Await Patterns
- Always use async/await for I/O operations
- Return `Task<T>` for async methods that return values
- Use `ConfigureAwait(false)` in library code (not needed in ASP.NET Core)
- Example: `public override Task<HelloReply> SayHello(...)` in GreeterService.cs:14

## Error Handling
- Use try-catch blocks for expected exceptions
- Log exceptions with appropriate log levels
- Return meaningful error responses to clients
- Use gRPC status codes for service errors

## Logging Best Practices
- Inject `ILogger<T>` into services for logging
- Use structured logging with parameters
- Log at appropriate levels (Debug, Information, Warning, Error)
- Example: `private readonly ILogger<GreeterService> _logger;` in GreeterService.cs:8

# gRPC-Specific Guidelines

## Protocol Buffer (.proto) Files
- Keep .proto files synchronized between client and server projects
- Use semantic versioning for API changes
- Add comments to .proto files explaining service methods
- Example: `Protos/greet.proto` defines the service contract

## Service Implementation
- Inherit from the generated `[ServiceName].ServiceNameBase` class
- Override RPC methods with proper signatures
- Use `ServerCallContext` for accessing request metadata
- Return appropriate gRPC status codes
- Example: `public class GreeterService : Greeter.GreeterBase` in GreeterService.cs:6

## Client Usage
- Create gRPC channels with proper configuration
- Dispose channels properly (use `using` statements)
- Handle gRPC exceptions appropriately
- Example: `using var channel = GrpcChannel.ForAddress("http://localhost:5117");` in Program.cs:34

## Testing gRPC Services
- Use `WebApplicationFactory<Program>` for integration tests
- Create test gRPC clients using the test server
- Test both success and error scenarios
- Example: Integration tests in `GrpcService1.Tests/Integration/`

# Testing Guidelines

## Unit Testing with xUnit
- Use descriptive test method names: `[Method]_[Scenario]_[ExpectedResult]`
- Follow AAA pattern: Arrange, Act, Assert
- Use `[Fact]` for single test cases, `[Theory]` for parameterized tests
- Example: `SayHello_WithValidRequest_ReturnsGreeting()` in GreeterServiceTests.cs:21

## Mocking with Moq
- Mock external dependencies in unit tests
- Verify method calls when testing behavior
- Use `Mock.Object` to get the mocked instance
- Example: `Mock<ILogger<GreeterService>> _mockLogger` in GreeterServiceTests.cs:11

## Integration Testing
- Use `WebApplicationFactory` for testing ASP.NET Core applications
- Test the complete request/response cycle
- Use real gRPC clients to test services end-to-end
- Example: `GrpcIntegrationTests` class in Integration folder

## Test Organization
- Group related tests in the same test class
- Use separate test projects for different components
- Name test projects with `.Tests` suffix
- Example: `GrpcService1.Tests`, `ConsoleApp1.Tests`

# Architecture Guidelines

## Builder Pattern Implementation
- Use private constructors with static factory methods
- Return the builder instance from setter methods for fluent chaining
- Implement immutable objects through builders
- Example: `OrderBuilder.Create().Id(1).Price(100).Build()` in Program.cs:21-29

## Domain Models
- Keep domain models simple with clear property definitions
- Initialize string properties to avoid null reference issues
- Use meaningful property names that reflect business concepts
- Example: `Address` class with `Street`, `City`, `State`, `Zip` in Order.cs:15-21

## Service Layer Patterns
- Keep services focused on single responsibilities
- Use dependency injection for service dependencies
- Implement interfaces for better testability
- Example: `GreeterService` implementing gRPC service logic

## Configuration Management
- Use the built-in configuration system in .NET
- Keep configuration values in appsettings.json
- Use strongly-typed configuration options when possible
- Access configuration through dependency injection

# Development Workflow

## Feature Development Process
1. Create feature branch from main: `git checkout -b feature/feature-name`
2. Implement changes following coding standards
3. Write comprehensive tests (unit + integration)
4. Ensure all tests pass: `dotnet test`
5. Push changes - CI/CD pipeline runs automatically
6. PR created automatically after successful pipeline
7. Code review and merge to main

## Code Review Guidelines
- Review for functionality, performance, and security
- Ensure tests cover new functionality
- Verify naming conventions are followed
- Check for proper error handling
- Validate gRPC service contracts are maintained

## Debugging Techniques
- Use breakpoints in Visual Studio or VS Code
- Enable detailed logging for troubleshooting
- Use gRPC client tools for testing services
- Monitor application performance with built-in tools

## Performance Considerations
- Minimize object allocations in hot paths
- Use async/await for I/O operations
- Consider connection pooling for gRPC clients
- Profile applications under realistic load

# Security Best Practices

## gRPC Security
- Use HTTPS/TLS for production deployments
- Implement authentication and authorization
- Validate all input parameters
- Use secure defaults for gRPC channel options

## Input Validation
- Validate all incoming request data
- Use data annotations for model validation
- Implement business rule validation
- Return appropriate error messages

## Logging Security
- Never log sensitive information (passwords, tokens, etc.)
- Use structured logging to avoid injection attacks
- Implement log levels appropriately
- Example: Careful logging in service implementations

## Dependency Management
- Keep NuGet packages updated (handled by Dependabot)
- Review security advisories for dependencies
- Use package vulnerability scanning
- Remove unused dependencies

## Development Notes

- Proto files are shared between client and server projects
- Client-side builder pattern is implemented for Order and Address entities
- Integration tests use TestServer to test the gRPC service end-to-end
- Server runs on port 5117 by default (configured in Program.cs:6-11)
- All code changes trigger comprehensive CI/CD pipeline
- PRs are automatically created after successful pipeline runs
- Follow the established patterns in the codebase for consistency