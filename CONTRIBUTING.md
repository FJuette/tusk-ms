# Contributing

Feel free to help me improving this template.

## Local testing

Local template install (osx, linux)
> dotnet new -i ./

Local template install (Windows)
> dotnet new -i .\

Find entry to uninstall
> dotnet new -u

Create example command (-n to provide new name of the solution)
> dotnet new ddd-webapi -n Magnus

## Local testing with docker

Build the container
> docker build -t tusk .

Run the container in dev mode
> docker run --rm -p 5100:80 -e ASPNETCORE_ENVIRONMENT=Development -e JWT_ISSUER=localhost -e JWT_KEY=123456 tusk

## Packing

Creating the nuget package
> nuget pack -NoDefaultExcludes

Install the nuget package locally
> dotnet new -i ./FJuette.Template.WebApi.1.0.X.nupkg

On _DateTimeOffset_ error:
> nuget locals all -clear
