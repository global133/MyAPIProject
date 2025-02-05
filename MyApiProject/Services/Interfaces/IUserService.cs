using MyApiProject.Models;
using MyApiProject.Models.DTOModels;


namespace MyApiProject.Services.Interfaces
{
    public interface IUserService
    {
        //получить всех пользователей
        Task<IEnumerable<DTOUser>> GetUsersAsync();

        //получить пользователя по id
        Task<DTOUser> GetUserByIdAsync(int id);

        //создать пользователя
        Task<DTOUser> CreateUserAsync(User user);

        //обновить данные пользователя 
        Task<DTOUser> UpdateUserAsync(int userId, User updateUser);

        //удалить пользователя
        Task<bool> DeleteUserAsync(int id);

        //получить пользователей которые работают над одним проектом
        Task<IEnumerable<DTOUser>> GetUsersInProjectAsync(int projectId);
    }
}
