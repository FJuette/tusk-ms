using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Tusk.Api.Models;
using Tusk.Api.Persistence;

namespace Tusk.Api.Tests.Common
{
    public class Utilities
    {
        public static StringContent GetRequestContent(object obj)
        {
            return new StringContent(JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json");
        }

        public static async Task<T> GetResponseContent<T>(HttpResponseMessage response)
        {
            var stringResponse = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<T>(stringResponse);

            return result;
        }

        public static void InitializeDbForTests(TuskDbContext context)
        {
            context.Stories.Add(
                new UserStory(
                    "My demo user story",
                    Priority.Create(1).Value,
                    "Info",
                    "Provide long text here",
                    BusinessValue.BV900));
            context.SaveChanges();
        }
    }
}
