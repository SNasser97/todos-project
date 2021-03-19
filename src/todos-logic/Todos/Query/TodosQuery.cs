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
            if (id == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(id));
            }

            Todo foundTodo = await this.todosRepository.GetTodoAsync(id);

            if (foundTodo == null)
            {
                throw new Exception("todo not found");
            }

            return foundTodo;
        }

        public async Task<IEnumerable<Todo>> GetTodosAsync()
        {
            IEnumerable<Todo> todos = await this.todosRepository.GetTodosAsync();

            return todos;
        }
    }
}