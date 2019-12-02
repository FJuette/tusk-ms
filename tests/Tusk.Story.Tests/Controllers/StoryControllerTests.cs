using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
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

            var vm = await Utilities.GetResponseContent<UserStoryViewModel>(response);

            // Assert
            vm.Stories.Count().Should().BeGreaterThan(0);
        }
    }
}
