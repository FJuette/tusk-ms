FROM mcr.microsoft.com/dotnet/aspnet:7.0-alpine AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /build
COPY src/. .
RUN dotnet restore ./Tusk.Api/Tusk.Api.csproj -nowarn:msb3202,nu1503
RUN dotnet build ./Tusk.Api/Tusk.Api.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish ./Tusk.Api/Tusk.Api.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
# Running the container as non root user ports < 1000 cannot be used
ENV ASPNETCORE_URLS=http://*:8080
RUN addgroup -S runners && adduser -S runner -G runners
USER runner:runners
ENTRYPOINT ["dotnet", "Tusk.Api.dll"]
