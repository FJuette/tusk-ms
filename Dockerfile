FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /build
COPY src/Tusk.Api/. .
RUN dotnet restore -nowarn:msb3202,nu1503
RUN dotnet build -c Release -o /app

FROM build AS publish
RUN dotnet publish -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
# Running the container as non root user ports < 1000 cannot be used
ENV ASPNETCORE_URLS=http://*:8080
RUN useradd runner && groupadd runners
USER runner:runners
ENTRYPOINT ["dotnet", "Tusk.Api.dll"]
