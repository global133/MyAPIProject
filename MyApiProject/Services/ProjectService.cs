using Microsoft.EntityFrameworkCore;
using MyApiProject.Data;
using MyApiProject.Models;
using MyApiProject.Models.DTOModels;
using MyApiProject.Services.Interfaces;


namespace MyApiProject.Services
{
    public class ProjectService : IProjectService
    {
        private readonly DatabaseConnection _context;

        public ProjectService(DatabaseConnection context)
        {
            _context = context;
        }

        //создать проект
        public async Task<DTOProject> CreateProjectAsync(Project project)
        {
            project.CreatedAt = DateTime.UtcNow;
            project.UpdatedAt = DateTime.UtcNow;
            _context.Projects.Add(project);
            await _context.SaveChangesAsync();

            var dto = new DTOProject
            {
                Id = project.Id,
                Name = project.Name,
                Description = project.Description
            };

            return dto;
        }

        //обновить проект
        public async Task<bool> UpdateProjectAsync(int id, Project project)
        {
            var existingProject = await _context.Projects.FindAsync(id);

            if (existingProject == null)
            {
                throw new KeyNotFoundException($"Project with id {id} not found");
            }

            existingProject.Name = project.Name;
            existingProject.Description = project.Description;
            existingProject.UpdatedAt = DateTime.UtcNow;

            _context.Projects.Update(existingProject);
            await _context.SaveChangesAsync();
            return true;
        }

        //получить список всех проектов
        public async Task<IEnumerable<DTOProject>> GetAllProjectsAsync()
        {
            var projects = await _context.Projects
                .Include(p  => p.Users)
                .Include(p => p.Tasks)
                .ToListAsync();

            if (!projects.Any())
            {
                throw new KeyNotFoundException($"Project not found");
            }

            var dtolist = projects.Select(project => new DTOProject
            {
                Id = project.Id,
                Name = project.Name,
                Description = project.Description,
                UserIds = project.Users.Select(u => u.Id).ToList(),
                TaskIds = project.Tasks.Select(t => t.Id).ToList()
            }).ToList();

            return dtolist;
        }

        //получить проект по id
        public async Task<DTOProject> GetProjectByIdAsync(int id)
        {
            var project = await _context.Projects.FindAsync(id);

            if (project == null)
            {
                throw new KeyNotFoundException($"Project with id {id} not found");
            }

            var dto = new DTOProject
            {
                Id = id,
                Name = project.Name,
                Description = project.Description,
            };

            return dto;
        }

        //добавить пользователя в проект
        public async Task<DTOUser> AddUserToProjectAsync(int projectId, int userId)
        {
            var project = await _context.Projects.Include(p => p.Users).FirstOrDefaultAsync(p => p.Id == projectId);
            var user = await _context.Users.FindAsync(userId);

            if (project == null || user == null)
            {
                throw new KeyNotFoundException($"Project or user with id project {projectId} or user {userId} not found");
            }

            if (!project.Users.Contains(user))
            {
                project.Users.Add(user);
                await _context.SaveChangesAsync();
            }

            var dto = new DTOUser
            {
                Username = user.Username,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
            };

            return dto; 
        }
        
        // Удалить пользователя из проекта
        public async Task<bool> RemoveUserFromProjectAsync(int projectId, int userId)
        {
            var project = await _context.Projects.Include(p => p.Users).FirstOrDefaultAsync(p => p.Id == projectId);
            var user = await _context.Users.FindAsync(userId);

            if (project == null || user == null)
            {
                return false; 
            }

            if (project.Users.Contains(user))
            {
                project.Users.Remove(user);
                await _context.SaveChangesAsync();
                return true;
            }

            return false; 
        }

        //получить пользователей по id проекта
        public async Task<IEnumerable<DTOUser>> GetUsersInProjectAsync(int projectId)
        {
            var project = await _context.Projects.Include(p => p.Users)
                .FirstOrDefaultAsync(p => p.Id == projectId);

            if (project == null)
            {
                throw new KeyNotFoundException($"Users with project id {projectId} not found.");
            }

            if (project.Users.Count() == 0)
            {
                throw new KeyNotFoundException($"Users with project id {projectId} not found.");
            }

            var dtolist = project.Users.Select(user => new DTOUser
            {
                Username = user.Username,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
            }).ToList();

            return dtolist;
        }

        //удалить проект
        public async Task<bool> DeleteProjectAsync(int id)
        {
            var project = await _context.Projects.FindAsync(id);

            if (project == null)
            {
                throw new KeyNotFoundException($"Project with id {id} not found");
            }

            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
