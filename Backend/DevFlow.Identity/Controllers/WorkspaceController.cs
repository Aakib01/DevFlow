using DevFlow.Identity.Entities;
using DevFlow.Identity.Infrastructure.Data;
using DevFlow.Identity.Processor;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DevFlow.Identity.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class WorkspaceController : ControllerBase
    {
        private readonly IIdentityProcessor _authProcessor;

        public WorkspaceController(IIdentityProcessor authProcessor)
        {
            _authProcessor = authProcessor;
        }

        [HttpPost]
        public async Task<IActionResult> Create(string name)
        {
            var userId = User.FindFirst("userId");
            var tenantId = User.FindFirst("tenantId");

            _authProcessor.CreateWorkspace(name, int.Parse(tenantId!.Value), int.Parse(userId!.Value));          

            return Ok();
        }

        [HttpPost("{workspaceId}/members")]
        public async Task<IActionResult> AddMember(int workspaceId, int userId, string role)
        {
            var currentUserId = User.FindFirst("userId")!.Value;

            if (!await _authProcessor.IsAdmin(int.Parse(currentUserId), workspaceId))
                return Forbid();

            await _authProcessor.AddWorkspaceMember(userId, workspaceId, role);
            
            return Ok();
        }

        [HttpPost("{workspaceId}/{memberId}")]
        public async Task<IActionResult> GetMemberById(int workspaceId, int memberId)
        {

            var result = await _authProcessor.GetMemberById(memberId);

            return Ok(result);
        }


    }
}
