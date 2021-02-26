using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace todos_data.Repository
{
    public interface ITodosRepository
    {
        // Repo handles only CRUD
        Task<Guid> CreateTodoAsync(Todo todo);
        Task<Todo> GetTodoAsync(Guid id);
        Task<IEnumerable<Todo>> GetTodosAsync();
        Task<Guid> UpdateTodoAsync(Todo todo);
        Task DeleteTodoAsync(Guid id);
    }
}