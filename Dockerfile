# Base stage
FROM mcr.microsoft.com/dotnet/runtime:9.0 AS base
WORKDIR /app

# Build step
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy the project files first
COPY src/Domain/*.csproj ./Domain/
COPY src/Application/*.csproj ./Application/
COPY src/Infrastructure/*.csproj ./Infrastructure/
COPY src/Presentation/*.csproj ./Presentation/

# Create temporary solution for restore
RUN dotnet new sln -n Temp && \
    dotnet sln add Domain/Domain.csproj && \
    dotnet sln add Application/Application.csproj && \
    dotnet sln add Infrastructure/Infrastructure.csproj && \
    dotnet sln add Presentation/Presentation.csproj

# Restore packages
RUN dotnet restore

# Copy the rest of the code
COPY . .

# Project build
RUN dotnet build src/Presentation/Presentation.csproj -c Release -o /app/build

# Publication
FROM build AS publish
RUN dotnet publish src/Presentation/Presentation.csproj -c Release -o /app/publish

# Final image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Presentation.dll"]
