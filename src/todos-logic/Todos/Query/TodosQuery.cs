using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using todos_data;
using todos_data.Repository;

namespace todos_logic.Todos.Query
{
    public class TodosQuery : ITodosQuery
    {
        private readonly ITodosRepository todosRepository;

        public TodosQuery(ITodosRepository todosRepository)
        {
            this.todosRepository = todosRepository ?? throw new ArgumentNullException(nameof(todosRepository));
        }

        public async Task<Todo> GetTodoAsync(Guid id)
        {
            Todo foundTodo = await this.todosRepository.GetTodoAsync(id);

            return foundTodo;
        }

        public Task<IEnumerable<Todo>> GetTodosAsync()
        {
            throw new NotImplementedException();
        }
    }
}