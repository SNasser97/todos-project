using System;
using System.Threading.Tasks;
using todos_data;
using todos_data.Repository;

namespace todos_logic.Todos.Command
{
    public class TodosCommand : ITodosCommand
    {
        public ITodosRepository TodosRepository { get; set; }
        
        public TodosCommand(ITodosRepository todosRepository)
        {
            this.TodosRepository = todosRepository ?? throw new ArgumentNullException(nameof(todosRepository));
        }

        public async Task<Guid> CreateTodoAsync(CreateTodoCommand todo)
        {
            if (todo == null)
            {
                throw new ArgumentNullException(nameof(todo));
            }
            
            Todo todoToCreate = todo.ToTodoData();

            Guid createdTodoId = await this.TodosRepository.CreateTodoAsync(todoToCreate);

            return await Task.FromResult(createdTodoId);
        }

        public Task DeleteTodoAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<Guid> UpdateTodoAsync(UpdateTodoCommand todo)
        {
            throw new NotImplementedException();
        }
    }
}