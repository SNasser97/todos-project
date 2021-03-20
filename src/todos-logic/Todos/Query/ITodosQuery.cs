using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using todos_data;

namespace todos_logic.Todos.Query
{
    public interface ITodosQuery
    {
        Task<Todo> GetTodoAsync(Guid id);

        Task<IEnumerable<Todo>> GetTodosAsync();
    }
}