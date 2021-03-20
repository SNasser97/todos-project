using System;
using todos_data;
using todos_logic.Todos.Command;

namespace todos_logic
{
    public class CreateTodoCommand : ICreateTodoCommand
    {
        public string Name { get; set; }

        public Todo ToDataObject()
        {
            return new Todo { Name = this.Name };
        }
    }
}