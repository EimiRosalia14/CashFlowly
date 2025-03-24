# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy the solution file and project files for dependency restoration
COPY CashFlowly.sln .
COPY CashFlowly.API/*.csproj ./CashFlowly.API/
COPY CashFlowly.Core.Application/*.csproj ./CashFlowly.Core.Application/
COPY CashFlowly.Core.Domain/*.csproj ./CashFlowly.Core.Domain/
COPY CashFlowly.Infrastructure.Persistence/*.csproj ./CashFlowly.Infrastructure.Persistence/

# Restore dependencies using the solution file
RUN dotnet restore CashFlowly.sln

# Copy the entire source code
COPY . .

# Publish the API project
RUN dotnet publish CashFlowly.API/CashFlowly.API.csproj -c Release -o out

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/out .

# Explicitly expose port 8080
EXPOSE 8080

# Configure ASP.NET Core to use the PORT environment variable
ENV ASPNETCORE_URLS=http://+:${PORT:-8080}

# Start the application
ENTRYPOINT ["dotnet", "CashFlowly.API.dll"]