# Backend Folder Guide

This document explains the backend structure in the WebApi project.

## Project Root

- backend/WebApi: Main ASP.NET Core API project root where startup, configuration, and backend code live.
- backend/WebApi/Program.cs: Application startup, dependency injection registration, and middleware pipeline.
- backend/WebApi/WebApi.csproj: Project file with dependencies and build settings.
- backend/WebApi/appsettings.json: Base application configuration.
- backend/WebApi/appsettings.Development.json: Development-only configuration overrides.

## API Layer

- backend/WebApi/Controllers: HTTP endpoint handlers. They accept requests, call services, and return responses.
- backend/WebApi/DTOs: API request and response models used at the HTTP boundary.

## Business Logic Layer

- backend/WebApi/Services: Core business logic for projects, interviews, and artifact generation.
- backend/WebApi/Interfaces: Service contracts to support clean dependency injection and testing.

## AI Layer

- backend/WebApi/AI: AI integration area.
- backend/WebApi/AI/Agents: Per-artifact generation agents.
- backend/WebApi/Options: Strongly typed option classes used for config binding.

## Data and Domain Layer

- backend/WebApi/Data: EF Core DbContext and persistence configuration.
- backend/WebApi/Entities: Domain and persistence models.
- backend/WebApi/Entities/Enum: Shared enums used across services and entities.
- backend/WebApi/Migrations: EF Core migration history and schema changes.

## Cross-Cutting Layer

- backend/WebApi/Middleware: Request pipeline components (for example global exception handling).
- backend/WebApi/Exceptions: Custom exception types for consistent error responses.
- backend/WebApi/Questions: Interview question catalog and question definitions.

## Runtime and Tooling

- backend/WebApi/Properties: Local launch profiles and debug settings.
- backend/WebApi/bin: Compiled output artifacts.
- backend/WebApi/obj: Intermediate build artifacts.
