FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["AccountServicesApi/AccountServicesApi.csproj", "AccountServicesApi/"]
RUN dotnet restore "AccountServicesApi/AccountServicesApi.csproj"
COPY . .
WORKDIR "/src/AccountServicesApi"
RUN dotnet build "AccountServicesApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AccountServicesApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AccountServicesApi.dll"]
