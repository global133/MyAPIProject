using MyApiProject.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using MyApiProject.Data;
using MyApiProject.Models;
using System.Data;
using MyApiProject.Models.DTOModels;


namespace MyApiProject.Services
{
    public class TaskService : ITaskService
    {
        private readonly DatabaseConnection _context;

        public TaskService(DatabaseConnection context)
        {
            _context = context;
        }

        //создать задачу в проекте
        public async Task<DTOTask> CreateTaskAsync(ProjectTask task, int projectId)
        {
            var project = await _context.Projects.FindAsync(projectId); 

            if (project == null)
            {
                throw new KeyNotFoundException($"Project with id {projectId} not found.");
            }

            var existingTask = await _context.Tasks
                .Where(t => t.Project.Id == projectId && t.Title == task.Title) 
                .FirstOrDefaultAsync();

            if (existingTask != null)
            {
                throw new InvalidOperationException($"Task with name '{task.Title}' already exists for this project.");
            }

            task.CreatedAt = DateTime.UtcNow;
            task.UpdatedAt = DateTime.UtcNow;

            task.Project = project; 

            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();

            var dto = new DTOTask
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                Status = task.Status,
            };

            return dto;
        }

        //обновить задачу в проекте
        public async Task<DTOTask> UpdateTaskAsync(int taskId, ProjectTask updatedTask, int projectId)
        {
            var task = await _context.Tasks.FirstOrDefaultAsync(t => t.Id == taskId && t.Project.Id == projectId); 

            if (task == null)
            {
                throw new KeyNotFoundException($"{taskId} not found");
            }

            task.Title = updatedTask.Title;
            task.Description = updatedTask.Description;
            task.Status = updatedTask.Status;
            task.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            var dto = new DTOTask
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                Status = task.Status,
            };

            return dto;
        }

        //получить список задач по id проекта
        public async Task<IEnumerable<DTOTask>> GetTasksByProjectIdAsync(int projectId)
        {
            var tasks = await _context.Tasks
                .Where(t => t.Project.Id == projectId) 
                .AsNoTracking()
                .ToListAsync();

            if (!tasks.Any())
            {
                throw new KeyNotFoundException($"Project with id {projectId} not found");
            }

            var dtolist = tasks.Select(task => new DTOTask
            {
                Id= task.Id,
                Title = task.Title,
                Description = task.Description,
                Status = task.Status,
            }).ToList();

            return dtolist;
        }

        //получить задачу по id
        public async Task<DTOTask> GetTaskByIdAsync(int taskId)
        {
            var task = await _context.Tasks.FindAsync(taskId);

            if (task == null)
            {
                throw new KeyNotFoundException($"Task with such id {taskId} not found");
            }

            var dto = new DTOTask
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                Status = task.Status,
            };

            return dto;
        }

        //получить список задач по статусу 
        public async Task<IEnumerable<DTOTask>> GetTasksByProjectIdAndStatusAsync(int projectId, string status)
        {
            var tasks = await _context.Tasks
                .Where(t => t.Project.Id == projectId && t.Status == status) 
                .ToListAsync();

            if (!tasks.Any())
            {
                throw new KeyNotFoundException($"Task with projectid {projectId} or status {status} not found");
            }

            var dtolist = tasks.Select(task => new DTOTask
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                Status = task.Status,
            }).ToList();

            return dtolist;
        }

        //удалить задачу из проекта
        public async Task<bool> DeleteTaskAsync(int taskId, int projectId)
        {
            var task = await _context.Tasks.FirstOrDefaultAsync(t => t.Id == taskId && t.Project.Id == projectId); 

            if (task == null)
            {
                return false;
            }

            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
