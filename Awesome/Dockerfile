# Use the official .NET SDK image as a build environment
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Set the working directory
WORKDIR /source

# Copy the project files to the working directory
COPY . .

# Clear NuGet cache
RUN dotnet nuget locals all --clear

# Restore dependencies
RUN dotnet restore

# Publish the application
RUN dotnet publish -c release -o /app --no-restore

# Use the official .NET runtime image for the application
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime

# Set the working directory
WORKDIR /app

# Copy the published output from the build environment
COPY --from=build /app .

ENV CONNECTION_STRING="DockerConnection"

# Expose the port the app runs on
EXPOSE 8000

# Run the application
ENTRYPOINT ["dotnet", "Awesome.dll"]
