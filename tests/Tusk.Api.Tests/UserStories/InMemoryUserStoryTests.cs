using Microsoft.EntityFrameworkCore;
using Tusk.Api.Persistence;

namespace Tusk.Api.Tests.Controllers;
public class InMemoryUserStoryTests : UserStoryTests
{
    public InMemoryUserStoryTests()
        : base(new DbContextOptionsBuilder<TuskDbContext>()
            .UseInMemoryDatabase(new Guid().ToString())
            .Options)
    {

    }
}
