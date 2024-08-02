FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
#EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["AppointmentSchedulingService/Consumers/API/API.csproj", "AppointmentSchedulingService/Consumers/API/"]
COPY ["AppointmentSchedulingService/Adpters/Data/Data.csproj", "AppointmentSchedulingService/Adpters/Data/"]
COPY ["AppointmentSchedulingService/Adpters/Infra/Infra.csproj", "AppointmentSchedulingService/Adpters/Infra/"]
COPY ["AppointmentSchedulingService/Core/Domain/Domain.csproj", "AppointmentSchedulingService/Core/Domain/"]
COPY ["AppointmentSchedulingService/Core/Application/Application.csproj", "AppointmentSchedulingService/Core/Application/"]
RUN dotnet restore "./AppointmentSchedulingService/Consumers/API/API.csproj"
COPY . .
WORKDIR "/src/AppointmentSchedulingService/Consumers/API"
RUN dotnet build "./API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "API.dll"]