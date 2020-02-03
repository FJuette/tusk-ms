using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Tusk.Story.Models;
using Tusk.Story.Stories.Commands;
using Tusk.Story.Stories.Queries;
using Tusk.Story.Tests.Common;
using Xunit;

namespace Tusk.Story.Tests.Controllers
{
    public class StoryControllerTests : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly HttpClient _client;

        public StoryControllerTests(CustomWebApplicationFactory<Startup> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task Stories_Success_ListOfStories()
        {
            // Act
            var response = await _client.GetAsync("api/stories");
            response.EnsureSuccessStatusCode();

            var vm = await Utilities.GetResponseContent<UserStoriesViewModel>(response);

            // Assert
            vm.Stories.Count().Should().BeGreaterThan(0);
            vm.Stories.First().Importance.Should().Be(UserStory.Relevance.MustHave);
        }

        [Fact]
        public async Task Create_Success_ReturnsStoryId()
        {
            // Act
            var response = await _client.PostAsJsonAsync("api/stories", new CreateStoryCommand
            {
                Importance = UserStory.Relevance.CouldHave,
                Text = "My demo post user story",
                Title = "Demo post"
            });
            response.EnsureSuccessStatusCode();

            var result = await Utilities.GetResponseContent<int>(response);

            // Assert
            result.Should().BeGreaterOrEqualTo(0);
        }

        [Fact]
        public async Task Create_InvalidImportance_ReturnsValidationError()
        {
            // Act
            var response = await _client.PostAsJsonAsync("api/stories", new
            {
                Importance = 5,
                Text = "My demo post user story",
                Title = "Demo post"
            });

            // Assert
            response.StatusCode.Should().Be(400);
        }

        [Fact]
        public async Task Story_Success_ProjectFound()
        {
            // Act
            var response = await _client.GetAsync("api/stories/1");
            response.EnsureSuccessStatusCode();

            var vm = await Utilities.GetResponseContent<UserStoryViewModel>(response);

            // Assert
            vm.Story.Should().NotBeNull();
        }

        [Fact]
        public async Task Story_InvalidId_ProjectNotFound()
        {
            // Act
            var response = await _client.GetAsync("api/stories/-100");

            // Assert
            response.StatusCode.Should().Be(404);
        }
    }
}
