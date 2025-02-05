using MyApiProject.Data;
using MyApiProject.Models;
using MyApiProject.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using MyApiProject.Models.DTOModels;


namespace MyApiProject.Services
{
    public class UserService : IUserService
    {
        private readonly DatabaseConnection _context;

        public UserService(DatabaseConnection context)
        {
            _context = context;
        }

        //создать пользователя
        public async Task<DTOUser> CreateUserAsync(User user)
        {
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == user.Email);

            if (existingUser != null)
            {
                throw new InvalidOperationException("A user with this email already exists.");
            }

            user.CreatedAt = DateTime.UtcNow;
            user.UpdatedAt = DateTime.UtcNow;

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var dtoUser = new DTOUser
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
            };

            return dtoUser;
        }

        //получить всех пользователей 
        public async Task<IEnumerable<DTOUser>> GetUsersAsync()
        {
            var users = await _context.Users.ToListAsync();
            
            if(!users.Any())
            {
                throw new KeyNotFoundException($"Users with not found");
            }
            
            var dtolist = users.Select(user => new DTOUser
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Username = user.Username,
            }).ToList();

            return dtolist;
        }

        //получить пользователя по id
        public async Task<DTOUser> GetUserByIdAsync(int userId)
        {
            var user = await _context.Users.FindAsync(userId);

            if(user == null)
            {
                throw new KeyNotFoundException($"User with such id {userId} not found");
            }

            var dtoUser = new DTOUser
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
            };

            return dtoUser;
        }

        //обновить данные пользователя
        public async Task<DTOUser> UpdateUserAsync(int userId, User updateUser)
        {
            var existingUser = await _context.Users.FindAsync(userId);

            if (existingUser == null)
            {
                throw new KeyNotFoundException($"User with such id {userId} not found");
            }

            existingUser.Username = updateUser.Username;
            existingUser.FirstName = updateUser.FirstName;
            existingUser.LastName = updateUser.LastName;
            existingUser.Email = updateUser.Email;
            existingUser.UpdatedAt = DateTime.UtcNow;

            _context.Users.Update(existingUser);
            await _context.SaveChangesAsync();

            var dtoUser = new DTOUser
            {
                Id = userId,
                Username = existingUser.Username,
                Email = existingUser.Email,
                FirstName = existingUser.FirstName,
                LastName = existingUser.LastName,
            };

            return dtoUser;
        }

        //удалить пользователя
        public async Task<bool> DeleteUserAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                throw new KeyNotFoundException($"User with such id {id} not found");
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }

        //получить пользователей, работающих над проектом
        public async Task<IEnumerable<DTOUser>> GetUsersInProjectAsync(int projectId)
        {
            var project = await _context.Projects
                .Include(p => p.Users)
                .FirstOrDefaultAsync(p => p.Id == projectId);

            if (project == null)
            {
                throw new KeyNotFoundException($"Project with id {projectId} not found");
            }

            var dtolist = project.Users.Select(user => new DTOUser
            {
                Id=user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Username = user.Username,
            }).ToList();

            return dtolist;
        }
    }
}
