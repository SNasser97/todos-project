using System;
using todos_data;

namespace todos_logic
{
    public class CreateTodoCommand
    {
        public string Name { get; set; }

        public Todo ToTodoData()
        {
            return new Todo { Name = this.Name };
        }
    }
}