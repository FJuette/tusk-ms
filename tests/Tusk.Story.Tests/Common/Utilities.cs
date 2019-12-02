using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Tusk.Story.Tests.Common
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

        //public static void InitializeDbForTests(ITuskDbContext context)
        //{
        //    context.Projects.Add(
        //        new Project
        //        {
        //            Name = "Example Project"
        //        }
        //    );
        //    context.SaveChanges();
        //}
    }
}
