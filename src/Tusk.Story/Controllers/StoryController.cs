using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Tusk.Story.Models;
using Tusk.Story.Stories.Commands;
using Tusk.Story.Stories.Queries;

namespace Tusk.Story.Controllers
{
    public class StoryController : BaseController
    {
        [HttpGet("api/stories")]
        [ProducesResponseType(200)]
        public async Task<ActionResult<UserStoryViewModel>> Stories()
        {
            return Ok(await Mediator.Send(new GetAllStoriesQuery()));
        }

        [HttpPost("api/stories")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<int>> Create([FromBody] CreateStoryCommand command)
        {
            var storyId = await Mediator.Send(command);
            return Ok(storyId);
        }
        /* 
                [HttpGet("api/projects/{id}")]
                [ProducesResponseType(200)]
                [ProducesResponseType(404)]
                public async Task<ActionResult<ProjectsListViewModel>> Project(int id)
                {
                    return Ok(await Mediator.Send(new GetProjectQuery(id)));
                }



                [HttpDelete("api/projects/{id}")]
                public async Task<ActionResult<int>> Delete(int id)
                {
                    var projectId = await Mediator.Send(new DeleteProjectCommand(id));
                    return Ok(projectId);
                }

                [HttpPut("api/projects")]
                public async Task<ActionResult<int>> Update([FromBody] UpdateProjectCommand command)
                {
                    var projectId = await Mediator.Send(command);
                    return Ok(projectId);
                }
                */
    }
}
