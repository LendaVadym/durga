# Multi-stage Dockerfile for Durga.SPA
# This Dockerfile should be run from the project root directory
# Build command: docker build -f server/Durga.SPA/Dockerfile -t durga-spa .

# Stage 1: Build Angular application
FROM node:20-alpine AS angular-build

# Set working directory for Angular app
WORKDIR /app/web-client

# Copy all web-client files and folders (excluding node_modules)
COPY server/Durga.SPA/web-client/ .

# Install dependencies (including dev dependencies needed for build)
RUN npm ci

# Build Angular app
RUN npm run build

# Stage 2: Build .NET application
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS dotnet-build

# Set working directory
WORKDIR /app

# Copy project file
COPY server/Durga.SPA/Durga.SPA.csproj ./

# Restore dependencies
RUN dotnet restore

# Copy .NET source code
COPY server/Durga.SPA/ ./

# Build and publish the application
ENV DOCKER_BUILD=true
RUN dotnet publish -c Release -o out --no-restore

# Stage 3: Runtime image
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime

# Set working directory
WORKDIR /app

# Create a non-root user
RUN adduser --disabled-password --gecos "" appuser && chown -R appuser /app
USER appuser

# Copy Angular build output from previous stage
COPY --from=angular-build /app/web-client/dist/durga-web-client/browser ./wwwroot

# Copy published application
COPY --from=dotnet-build --chown=appuser:appuser /app/out ./

# Expose port
EXPOSE 8080

# Set environment variables
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production

# Health check
HEALTHCHECK --interval=30s --timeout=10s --start-period=5s --retries=3 \
    CMD curl -f http://localhost:8080/health || exit 1

# Entry point
ENTRYPOINT ["dotnet", "Durga.SPA.dll"]