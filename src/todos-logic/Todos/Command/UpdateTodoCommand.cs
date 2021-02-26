using System;
using todos_data;

namespace todos_logic.Todos.Command
{
    public class UpdateTodoCommand
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public bool IsComplete { get; set; }

        public long UpdatedAt { get; set; }
        
        public Todo ToTodoData()
        {
            return new Todo
            {
                Id = this.Id,
                Name = this.Name,
                UpdatedAt = this.UpdatedAt,
                IsComplete = this.IsComplete
            };
        }
    }
}