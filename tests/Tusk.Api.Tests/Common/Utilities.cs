﻿using System.Net.Http;
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
            context.Stories.Add(new UserStory
            {
                Id = 1,
                AcceptanceCriteria = "Provide long text here",
                BusinessValue = 1000,
                Priority = 1,
                Text = "Info",
                Title = "My demo user story",
                Importance = UserStory.Relevance.ShouldHave
            });
            context.SaveChanges();
        }
    }
}