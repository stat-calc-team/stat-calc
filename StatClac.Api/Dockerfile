FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["StatClac.Api/StatClac.Api.csproj", "StatClac.Api/"]
RUN dotnet restore "StatClac.Api/StatClac.Api.csproj"
COPY . .
WORKDIR "/src/StatClac.Api"
RUN dotnet build "StatClac.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "StatClac.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "StatClac.Api.dll"]
