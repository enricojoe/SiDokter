FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy the main project file
COPY ["SiDokter/SiDokter.csproj", "SiDokter/"]

# Copy class library projects
COPY ["Services/Services.csproj", "Services/"]
COPY ["SiDokterRepository/Repositories.csproj", "SiDokterRepository/"]
COPY ["Entities/Entities.csproj", "Entities/"]

# Restore dependencies for the main project and class libraries
RUN dotnet restore "SiDokter/SiDokter.csproj"

# Copy everything else
COPY . .

# Build the main project
WORKDIR "/src/SiDokter"
RUN dotnet build "SiDokter.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SiDokter.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
VOLUME ["/var/data-protection-keys"]
ENTRYPOINT ["dotnet", "SiDokter.dll"]