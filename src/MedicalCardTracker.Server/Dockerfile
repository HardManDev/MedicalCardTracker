FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 5236

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/MedicalCardTracker.Server/MedicalCardTracker.Server.csproj", "src/MedicalCardTracker.Server/"]
COPY ["src/MedicalCardTracker.Application.Server/MedicalCardTracker.Application.Server.csproj", "src/MedicalCardTracker.Application.Server/"]
COPY ["src/MedicalCardTracker.Application/MedicalCardTracker.Application.csproj", "src/MedicalCardTracker.Application/"]
COPY ["src/MedicalCardTracker.Domain/MedicalCardTracker.Domain.csproj", "src/MedicalCardTracker.Domain/"]
COPY ["src/MedicalCardTracker.Database/MedicalCardTracker.Database.csproj", "src/MedicalCardTracker.Database/"]
RUN dotnet restore "src/MedicalCardTracker.Server/MedicalCardTracker.Server.csproj"
COPY . .
WORKDIR "/src/src/MedicalCardTracker.Server"
RUN dotnet build "MedicalCardTracker.Server.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "MedicalCardTracker.Server.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MedicalCardTracker.Server.dll"]
