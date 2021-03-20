using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using todos_data.TimestampModel;

namespace todos_data.Repository
{
    public class InMemoryTodosRepository : ITodosRepository
    {
        private readonly IDictionary<Guid, Todo> todos;
        private readonly ITimestamp timestamp;

        public InMemoryTodosRepository() : this(new ConcurrentDictionary<Guid, Todo>(), new Timestamp())
        {

        }

        public InMemoryTodosRepository(IDictionary<Guid, Todo> todos, ITimestamp timestamp)
        {
            this.todos = todos ?? throw new ArgumentNullException(nameof(todos));
            this.timestamp = timestamp ?? throw new ArgumentNullException(nameof(timestamp));
        }

        public async Task<Guid> CreateTodoAsync(Todo todo)
        {

            long newTimestamp = this.timestamp.TimestampInMiliseconds;
            todo.CreatedAt = newTimestamp;
            todo.UpdatedAt = newTimestamp;

            this.todos.Add(todo.Id, todo);

            return await Task.FromResult(todo.Id);
        }

        public async Task DeleteTodoAsync(Guid id)
        {
            if (this.todos.ContainsKey(id))
            {
                await Task.FromResult(this.todos.Remove(id));
            }
        }

        public async Task<Todo> GetTodoAsync(Guid id)
        {
            KeyValuePair<Guid, Todo> found = this.todos.FirstOrDefault(kvp => kvp.Key == id);

            return await Task.FromResult(found.Value);
        }

        public async Task<IEnumerable<Todo>> GetTodosAsync()
        {
            return await Task.FromResult(this.todos.Values);
        }

        public async Task<Guid> UpdateTodoAsync(Todo todo)
        {
            if (this.todos.TryGetValue(todo.Id, out Todo existingTodo))
            {
                // Only update timestamp prop
                existingTodo.Name = todo.Name;
                existingTodo.UpdatedAt = this.timestamp.TimestampInMiliseconds;
                existingTodo.IsComplete = todo.IsComplete;

                return await Task.FromResult(existingTodo.Id);
            }

            return await Task.FromResult(Guid.Empty);
        }
    }
}