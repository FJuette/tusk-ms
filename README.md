# Tusk
Inspired from https://github.com/JasonGT/NorthwindTraders


## TODOs

  * [x] Add FluentValidation -> see **CreateProjectCommandHandler**
  * [x] Add Validation error handling in exception filter -> not needed, only fluentvalidation is used -> **CustomExceptionFilter**
  * [x] Add automapper -> see **GetProjectQuery**
  * [x] Add swagger ->  see **ProjectController**
  * [ ] Add useful xunit unit tests with fluentassertations
  * [ ] Add userful xunit integration tests (api calls)
  * [x] Add Delete example
  * [x] Update example
  * [x] Add serilog as logging provider
  * [x] Add health status (https://medium.com/it-dead-inside/implementing-health-checks-in-asp-net-core-a8331d16a180)
  * [ ] Add business logic example
  * [ ] Add epics, stories
  * [ ] Add user and roles
  * [ ] Add a nice angular spa with material from ngx template (https://github.com/ngx-rocket/generator-ngx-rocket)
  * [ ] User Health checks https://medium.com/it-dead-inside/implementing-health-checks-in-asp-net-core-a8331d16a180

### Maybes

  * [ ] Add GraphQL
  * [ ] Add Identity Server
  * [ ] Add NServiceBus or RabbitMQ

## Technologies

  * ASP.NET & .Net Core 2.2
  * EF Core 2.2
  * FluentValidation
  * AutoMapper
  * MediatR
  * Swashbuckle Swagger
  * Serilog

## License

This project is licensed under the Apache License - see the [LICENSE](https://github.com/FJuette/tusk-ms/blob/master/LICENSE) file for details.
