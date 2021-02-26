using System;
using System.Threading.Tasks;

namespace todos_logic.Todos.Command
{
    public interface ITodosCommand
    {
        Task<Guid> CreateTodoAsync(CreateTodoCommand todo);
        Task<Guid> UpdateTodoAsync(UpdateTodoCommand todo);
        Task DeleteTodoAsync(Guid id);
    }
}