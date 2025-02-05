using MyApiProject.Models;
using MyApiProject.Models.DTOModels;


namespace MyApiProject.Services.Interfaces
{
    public interface ITaskService
    {
        Task<IEnumerable<DTOTask>> GetTasksByProjectIdAsync(int projectId);
        Task<DTOTask> GetTaskByIdAsync(int taskId);
        Task<DTOTask> CreateTaskAsync(ProjectTask task, int projectId);
        Task<DTOTask> UpdateTaskAsync(int taskId, ProjectTask updatedTask, int projectId);
        Task<bool> DeleteTaskAsync(int taskId, int projectId);
        Task<IEnumerable<DTOTask>> GetTasksByProjectIdAndStatusAsync(int projectId, string status);
    }
}
