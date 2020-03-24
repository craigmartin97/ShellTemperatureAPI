#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["ShellTemperatureAPI.csproj", "ShellTemperatureAPI/"]
COPY ["ShellTemperature.Repository/ShellTemperature.Repository.csproj", "ShellTemperature.Repository/"]
COPY ["ShellTemperature.Data/ShellTemperature.Data.csproj", "ShellTemperature.Data/"]
RUN dotnet restore "ShellTemperatureAPI/ShellTemperatureAPI.csproj"
COPY . .
WORKDIR "/src/ShellTemperatureAPI"
RUN dotnet build "ShellTemperatureAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ShellTemperatureAPI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ShellTemperatureAPI.dll"]