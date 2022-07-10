FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["EC2OnDemandGameHostingAPI/EC2OnDemandGameHostingAPI.csproj", "EC2OnDemandGameHostingAPI/"]
RUN dotnet restore "EC2OnDemandGameHostingAPI/EC2OnDemandGameHostingAPI.csproj"
COPY . .
WORKDIR "/src/EC2OnDemandGameHostingAPI"
RUN dotnet build "EC2OnDemandGameHostingAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "EC2OnDemandGameHostingAPI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EC2OnDemandGameHostingAPI.dll"]
