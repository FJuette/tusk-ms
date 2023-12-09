run:
	dotnet watch run --project ./src/Tusk.Api/Tusk.Api.csproj

test:
	dotnet test

docker:
	docker build -t tusk-ms .
	docker run --rm -p 5400:8080 -e CONNECTION_STRING=dummy -e ASPNETCORE_ENVIRONMENT=Development tusk-ms

pack:
	nuget pack FJuette.Template.WebApi.nuspec