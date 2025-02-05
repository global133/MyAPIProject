using MyApiProject.Models;
using MyApiProject.Models.DTOModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApiProject.Services.Interfaces
{
    public interface ICommentService
    {
        //получить список комментариев 
        Task<IEnumerable<DTOComment>> GetCommentsByTaskIdAsync(int taskId);

        //получить комментарий по id
        Task<DTOComment> GetCommentByIdAsync(int commentId);

        //создать комментарий и добавить к задаче
        Task<DTOComment> CreateCommentToTaskAsync(int taskId, Comment comment);

        //обновить информацию комментария
        Task<DTOComment> UpdateCommentAsync(int taskId, Comment comment, int commentId);

        //удалить комментарий
        Task<bool> DeleteCommentAsync(int commentId, int taskId);
    }
}
