using Microsoft.EntityFrameworkCore;
using Tusk.Api.Persistence;
using Xunit;

namespace Tusk.Api.Tests.UserStories;

[Collection("Sequential")]
public class InMemoryUserStoryTests() : UserStoryTests(new DbContextOptionsBuilder<TuskDbContext>()
    .UseInMemoryDatabase(Guid.NewGuid().ToString())
    .Options);
