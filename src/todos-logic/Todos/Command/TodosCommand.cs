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

            if (string.IsNullOrWhiteSpace(todo.Name))
            {
                throw new Exception("todo name was empty");
            }

            Todo todoToCreate = todo.ToDataObject();

            Guid createdTodoId = await this.TodosRepository.CreateTodoAsync(todoToCreate);

            return await Task.FromResult(createdTodoId);
        }

        public async Task DeleteTodoAsync(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(id));
            }

            Todo todo = await this.TodosRepository.GetTodoAsync(id);

            if (todo == null)
            {
                throw new Exception("todo not found");
            }

            await this.TodosRepository.DeleteTodoAsync(todo.Id);
        }

        public async Task<Guid> UpdateTodoAsync(UpdateTodoCommand todo)
        {
            if (todo == null)
            {
                throw new ArgumentNullException(nameof(todo));
            }

            if (string.IsNullOrWhiteSpace(todo.Name))
            {
                throw new Exception("todo name was empty");
            }

            if (todo.Id == Guid.Empty)
            {
                throw new Exception("todo Id is empty");
            }

            Todo foundTodo = await this.TodosRepository.GetTodoAsync(todo.Id);

            if (foundTodo == null)
            {
                throw new Exception("todo not found");
            }

            Todo todoData = todo.ToDataObject();

            Guid todoId = await this.TodosRepository.UpdateTodoAsync(todoData);

            return todoId;
        }
    }
}