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

        public Task<Todo> GetTodoAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Todo>> GetTodosAsync()
        {
            throw new NotImplementedException();
        }
    }
}