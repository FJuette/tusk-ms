using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tusk.Api.Stories.Commands;
using Tusk.Api.Stories.Queries;

namespace Tusk.Api.Controllers
{
    public class StoryController : BaseController
    {
        /// <summary>
        /// Get all stories
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/stories")]
        [ProducesResponseType(200)]
        public async Task<ActionResult<UserStoriesViewModel>> GetAllStories()
        {
            return Ok(await Mediator.Send(new GetAllStoriesQuery()));
        }

        /// <summary>
        /// Create a user story
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     {
        ///         "title": "My first story",
        ///         "text": "This is the very long content of my first story",
        ///         "importance": 1
        ///     }
        /// </remarks>
        /// <param name="command"></param>
        /// <returns>Id for the new user story</returns>
        /// <response code="201">Returns the id of the new user story</response>
        /// <response code="400">A request which cannot be handles properly returns a 400 with a detailed error message</response>
        [HttpPost("api/stories")]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<int>> CreateStory([FromBody] CreateStoryCommand command)
        {
            var storyId = await Mediator.Send(command);
            return CreatedAtAction("GetStory", new {id = storyId },  storyId);
        }

        [HttpGet("api/stories/{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<UserStoryViewModel>> GetStory(int id)
        {
            return Ok(await Mediator.Send(new GetStoryQuery(id)));
        }
    }
}
