run:
	dotnet watch run --project ./src/Tusk.Api/Tusk.Api.csproj

test:
	dotnet test

docker:
	docker build -t tusk-ms .
	docker run --rm -p 5400:8080 -e CONNECTION_STRING=dummy -e ASPNETCORE_ENVIRONMENT=Development tusk-ms

# make ef-add name=init
ef-add:
	cd ./src/Tusk.Api && dotnet ef migrations add $(name) --context TuskDbContext

ef-commit:
	cd ./src/Tusk.Api && dotnet ef database update --context TuskDbContext

pack:
	nuget pack FJuette.Template.WebApi.nuspec -NoDefaultExcludes
