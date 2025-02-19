FROM mcr.microsoft.com/dotnet/aspnet:9.0-alpine AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /build
COPY src/. .
COPY .editorconfig .
RUN dotnet restore ./Tusk.Api/Tusk.Api.csproj -nowarn:msb3202,nu1503
RUN dotnet build ./Tusk.Api/Tusk.Api.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish ./Tusk.Api/Tusk.Api.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Tusk.Api.dll"]
