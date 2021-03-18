using System;
using todos_data;

namespace todos_logic.Todos.Command
{
    public interface ITodoCommand<T>
    {
        T ToTodoData();
    }

    // Update
    public interface IUpdateTodoCommand : ITodoCommand<Todo>
    {
        Guid Id { get; set; }
        string Name { get; set; }
        bool IsComplete { get; set; }
    }

    // Create
    public interface ICreateTodoCommand : ITodoCommand<Todo>
    {
        string Name { get; set; }
    }
}