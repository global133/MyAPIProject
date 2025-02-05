using MyApiProject.Data;
using MyApiProject.Models;
using MyApiProject.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using MyApiProject.Models.DTOModels;

namespace MyApiProject.Services
{
    public class CommentService : ICommentService
    {
        private readonly DatabaseConnection _context;

        public CommentService(DatabaseConnection context)
        {
            _context = context;
        }

        //создать и добавить комментарий к задаче
        public async Task<DTOComment> CreateCommentToTaskAsync(int taskId, Comment comment)
        {
            var task = await _context.Tasks.FindAsync(taskId);

            if (task == null)
            {
                throw new KeyNotFoundException($"Task with id {taskId} not found");
            }

            comment.CreatedAt = DateTime.UtcNow;
            comment.UpdatedAt = DateTime.UtcNow;

            comment.Task = task;

            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();

            var dto = new DTOComment
            {
                Id = comment.Id,
                Text = comment.Text,
            };

            return dto;
        }

        //обновить информацию комментария 
        public async Task<DTOComment> UpdateCommentAsync(int taskId, Comment comment, int commentId)
        {
            var existingComment = await _context.Comments
                .FirstOrDefaultAsync(c => c.Id == commentId && c.Task.Id == taskId);

            if (existingComment == null)
            {
                throw new KeyNotFoundException($"Comment with suck task id {taskId} or comment id {commentId}not found");
            }

            existingComment.Text = comment.Text;
            existingComment.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            var dto = new DTOComment
            {
                Id = comment.Id,
                Text = comment.Text,
            };

            return dto;
        }

        //получить список комментариев по id задачи
        public async Task<IEnumerable<DTOComment>> GetCommentsByTaskIdAsync(int taskId)
        {
            var hasComments = await _context.Comments.AnyAsync(c => c.Task.Id == taskId);

            if (!hasComments)
            {
                throw new KeyNotFoundException("No comments found for this task.");
            }

            var comments = await _context.Comments
                .Include(c => c.Task)
                .Where(c => c.Task.Id == taskId)
                .ToListAsync();

            var dtolist = comments.Select(comment => new DTOComment
            {
                Id= comment.Id,
                Text = comment.Text,
            }).ToList();

            return dtolist;
        }

        //получить комментарий по id
        public async Task<DTOComment> GetCommentByIdAsync(int commentId)
        {
            var comment = await _context.Comments.FindAsync(commentId);

            if (comment == null)
            {
                throw new KeyNotFoundException($"Comment with id {commentId} not found.");
            }

            var dto = new DTOComment
            {
                Id = comment.Id,
                Text = comment.Text,
            };

            return dto;
        }

        //удалить комментарий 
        public async Task<bool> DeleteCommentAsync(int commentId, int taskId)
        {
            var comment = await _context.Comments.Where(c => c.Id == commentId && c.Task.Id == taskId).FirstOrDefaultAsync();

            if (comment == null)
            {
                throw new KeyNotFoundException($"Comment with comment id {commentId} or task id {taskId} not found");
            }

            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
