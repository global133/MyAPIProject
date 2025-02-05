using MyApiProject.Models;
using MyApiProject.Models.DTOModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApiProject.Services.Interfaces
{
    public interface IProjectService
    {
        // Создать новый проект.
        Task<DTOProject> CreateProjectAsync(Project project);

        // Обновить существующий проект.
        Task<bool> UpdateProjectAsync(int id, Project project);

        // Удалить проект.
        Task<bool> DeleteProjectAsync(int id);

        // Получить все проекты.
        Task<IEnumerable<DTOProject>> GetAllProjectsAsync();

        // Получить проект по ID.
        Task<DTOProject> GetProjectByIdAsync(int id);

        // Добавить пользователя в проект.
        Task<DTOUser> AddUserToProjectAsync(int projectId, int userId);

        // Удалить пользователя из проекта.
        Task<bool> RemoveUserFromProjectAsync(int projectId, int userId);

        // Получить всех пользователей проекта.
        Task<IEnumerable<DTOUser>> GetUsersInProjectAsync(int projectId);
    }
}
