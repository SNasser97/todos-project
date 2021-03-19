using System;
using todos_data;

namespace todos_logic.Todos.Command
{
    public class UpdateTodoCommand : IUpdateTodoCommand
    {
        public Guid Id { get; set; }
        
        public string Name { get; set; }

        public bool IsComplete { get; set; }

        public Todo ToDataObject()
        {
            return new Todo
            {
                Id = this.Id,
                Name = this.Name,
                IsComplete = this.IsComplete
            };
        }
    }
}