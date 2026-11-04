# Multi-stage build for .NET 10 API

# Stage 1: Build .NET API
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS dotnet-builder
WORKDIR /src

# Copy project files
COPY server/api/api.csproj ./server/api/
COPY server/be/be.csproj ./server/be/
COPY server/db/db.csproj ./server/db/
COPY service/service.csproj ./service/
COPY log_in_platform.sln ./

# Restore dependencies
RUN dotnet restore log_in_platform.sln

# Copy source code
COPY server/ ./server/
COPY service/ ./service/

# Build and publish .NET app
RUN dotnet publish server/api/api.csproj -c Release -o /app/publish

# Stage 2: Runtime - .NET API only
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app

# curl is used by the Compose health check.
RUN apt-get update \
    && apt-get install -y --no-install-recommends curl \
    && rm -rf /var/lib/apt/lists/*

# Create non-root user for security
RUN groupadd -r dotnet && useradd -r -g dotnet dotnet

# Copy published .NET app from builder
COPY --from=dotnet-builder /app/publish .

# Change ownership to non-root user
RUN chown -R dotnet:dotnet /app

# Switch to non-root user
USER dotnet

# Health check
HEALTHCHECK --interval=30s --timeout=10s --start-period=5s --retries=3 \
    CMD dotnet /app/api.dll --version || exit 1

# Expose port
EXPOSE 5056

# Set environment
ENV ASPNETCORE_HTTP_PORTS=5056
ENV ASPNETCORE_URLS=http://+:5056

# Run the application
ENTRYPOINT ["dotnet", "api.dll"]
