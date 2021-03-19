using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using todos_data;
using todos_data.Repository;
using todos_logic.Todos.Query;
using Xunit;

namespace todos_tests.Query
{
    public class GetTodosQueryTests
    {
        [Fact]
        public async Task TodosQueryGetTodosAsyncExpectsToReturnAListOfTodos()
        {
            // Given I have an expected todos list
            var expectedTodosList = new List<Todo>
            {
                new Todo { Id = Guid.NewGuid(), Name = "My todo", IsComplete = false, CreatedAt = 1234, UpdatedAt = 33245 },
                new Todo { Id = Guid.NewGuid(), Name = "My todo1", IsComplete = true, CreatedAt = 2234, UpdatedAt = 333245 },
                new Todo { Id = Guid.NewGuid(), Name = "My todo2", IsComplete = false, CreatedAt = 4234, UpdatedAt = 5353245 },
                new Todo { Id = Guid.NewGuid(), Name = "My todo3", IsComplete = true, CreatedAt = 2234, UpdatedAt = 2234 },
            };

            // And I have a mock todos repo
            var mockTodoRepo = new Mock<ITodosRepository>();

            // And I have an instance of todosQuery
            var todosQuery = new TodosQuery(mockTodoRepo.Object);
            mockTodoRepo.Setup(s => s.GetTodosAsync()).ReturnsAsync(expectedTodosList);

            // When I call GetTodosAsync
            IEnumerable<Todo> actualTodosList = await todosQuery.GetTodosAsync();

            // Then I expect a list of todos
            Assert.NotEmpty(actualTodosList);

            // Verify
            int actualTodosListLength = actualTodosList.Count();
            int expectedTodosListLength = expectedTodosList.Count;

            Assert.Equal(expectedTodosListLength, actualTodosListLength);

            // Verify each todo
            foreach (Todo actualTodo in actualTodosList)
            {
                Todo expectedTodo = expectedTodosList.FirstOrDefault(t => t.Id == actualTodo.Id);
                Assert.NotNull(expectedTodo);
                Assert.Equal(expectedTodo.Name, actualTodo.Name);
                Assert.Equal(expectedTodo.IsComplete, actualTodo.IsComplete);
                Assert.Equal(expectedTodo.CreatedAt, actualTodo.CreatedAt);
                Assert.Equal(expectedTodo.UpdatedAt, actualTodo.UpdatedAt);
            }
        }
    }
}