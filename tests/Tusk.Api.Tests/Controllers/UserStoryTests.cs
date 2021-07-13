using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Tusk.Api.Infrastructure;
using Tusk.Api.Models;
using Tusk.Api.Persistence;
using Tusk.Api.Stories.Queries;
using Tusk.Api.Tests.Common;
using Xunit;

namespace Tusk.Api.Tests.Controllers
{
    public abstract class UserStoryTests
    {
        public UserStoryTests(DbContextOptions<TuskDbContext> contextOptions)
        {
            ContextOptions = contextOptions;

            Seed();
        }
        protected DbContextOptions<TuskDbContext> ContextOptions { get; }

        private void Seed()
        {
            using var context = new TuskDbContext(ContextOptions);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            context.Stories.Add(
            new UserStory(
                "My demo user story",
                Priority.Create(1).Value,
                "Info",
                "Provide long text here",
                BusinessValue.BV900));
            context.SaveChanges();

        }

        [Fact]
        public async Task My_Simple_Test()
        {
            using var context = new TuskDbContext(ContextOptions);
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(typeof(UserStoriesProfile));
            });
            var mapper = new Mapper(configuration);

            var handler = new GetAllStoriesQueryHandler(context, mapper, FakeFactory.GetDtInstance());

            var result = await handler.Handle(new GetAllStoriesQuery(), new System.Threading.CancellationToken());

            result.Data.Length().Should().Be(1);
        }

    }
}
