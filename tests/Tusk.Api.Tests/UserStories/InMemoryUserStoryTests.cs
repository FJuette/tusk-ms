using Microsoft.EntityFrameworkCore;
using Tusk.Api.Persistence;
using Xunit;

namespace Tusk.Api.Tests.Controllers;

[Collection("Sequential")]
public class InMemoryUserStoryTests : UserStoryTests
{
    public InMemoryUserStoryTests()
        : base(new DbContextOptionsBuilder<TuskDbContext>()
            .UseInMemoryDatabase(new Guid().ToString())
            .Options)
    {

    }
}
