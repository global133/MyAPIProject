using Microsoft.AspNetCore.Mvc;
using MyApiProject.Models;
using MyApiProject.Services.Interfaces;


namespace MyApiProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly ICommentService _commentService;

        public CommentsController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        // GET: api/comments/task/{taskId}
        [HttpGet("task/{taskId}")]
        public async Task<ActionResult<IEnumerable<Comment>>> GetCommentsByTaskId(int taskId)
        {
            try
            {
                var comments = await _commentService.GetCommentsByTaskIdAsync(taskId);
                return Ok(comments);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        // GET: api/comments/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Comment>> GetComment(int id)
        {
            try
            {
                var comment = await _commentService.GetCommentByIdAsync(id);
                return Ok(comment);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        // POST: api/comments/task/{taskId}
        [HttpPost("task/{taskId}")]
        public async Task<ActionResult<Comment>> CreateComment(int taskId, [FromBody] Comment comment)
        {
            try
            {
                var createdComment = await _commentService.CreateCommentToTaskAsync(taskId, comment);
                return CreatedAtAction(nameof(GetComment), new {id = createdComment.Id }, createdComment);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        // PUT: api/comments/{taskId}/comment/{commentId}
        [HttpPut("{taskId}/comment/{commentId}")]
        public async Task<IActionResult> UpdateComment(int taskId, Comment comment, int commentId)
        {
            try
            {
                var success = await _commentService.UpdateCommentAsync(taskId, comment, commentId);
                return Ok(success);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }

        }

        // DELETE: api/comments/{id}
        [HttpDelete("{taskid}/comment/{commentId}")]
        public async Task<IActionResult> DeleteComment(int taskId, int commentId)
        {
            try
            {
                var success = await _commentService.DeleteCommentAsync(commentId, taskId);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });

            }
        }
    }
}
