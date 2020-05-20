# C# Web-Api template in DDD-Style for Microservices

My template for new projects in the world of microservices.

## Technologies

* .Net Core 3.1
* EF Core 3.1
* FluentValidation and .Net Core default validation disabled
* AutoMapper
* MediatR for CQRS
* Swashbuckle Swagger with xml documention enabled
* Serilog with default console output
* Unit and Integration Tests with XUnit
* DDD-Style Model Classes and Onion Architecture
* C# 8 Features enabled with nullable compiler checks

Mainly inspired by <https://github.com/JasonGT/NorthwindTraders> and Vladimir Khorikov (<https://github.com/vkhorikov>)

## Usage

Install the template from nuget
> dotnet new --install FJuette.Template.WebApi

Create a new project from the template
> dotnet new ddd-webapi -n Magnus

Remove the template
> dotnet new -u FJuette.Template.WebApi

## Remarks

I did not added a respository / UoW pattern because in my opinion its to heavy for microservices.

## References

* <https://medium.com/agilix/entity-framework-core-enums-ee0f8f4063f2>
* <https://code-maze.com/migrations-and-seed-data-efcore/>
* <https://dev.to/integerman/eliminating-nulls-in-c-with-functional-programming-iaa>
* <https://medium.com/swlh/safer-code-with-c-8-non-null-reference-types-cd5241e5714>
* <https://medium.com/swlh/getting-lazy-in-c-ccc5fd59cb7c>
* <https://herbertograca.com/2017/10/19/from-cqs-to-cqrs/>
* <http://docs.automapper.org/en/stable/Queryable-Extensions.html>
* <https://herbertograca.com/2017/11/16/explicit-architecture-01-ddd-hexagonal-onion-clean-cqrs-how-i-put-it-all-together/>

## License

This project is licensed under the Apache License - see the [LICENSE](https://github.com/FJuette/tusk-ms/blob/master/LICENSE) file for details.
