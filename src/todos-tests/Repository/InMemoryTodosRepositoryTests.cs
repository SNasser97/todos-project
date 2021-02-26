using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using todos_data;
using todos_data.Facade;
using todos_data.Repository;
using todos_tests.Utility;
using Xunit;

namespace todos_tests.Repository
{
    public class InMemoryTodosRepositoryTests
    {
        [Fact]
        public void TodosRepositoryTakesNullDictionaryAndExpectsToThrowArgumentNullException()
        {
            Exceptions.HandleExceptions<ArgumentNullException>(() =>
                new InMemoryTodosRepository(null as Dictionary<Guid, Todo>, new TimestampFacade()),
                (ex) => Assert.Equal("todos", ex.ParamName)
            );
        }

        [Fact]
        public void TodosRepositoryTakesNullTimestampFacadeAndExpectsToThrowArgumentNullException()
        {
            Exceptions.HandleExceptions<ArgumentNullException>(() =>
                new InMemoryTodosRepository(new Dictionary<Guid, Todo>(), null as TimestampFacade),
                (ex) => Assert.Equal("timestampFacade", ex.ParamName)
            );
        }

        [Fact]
        public async Task TodosRepositoryCreateTodoAsyncCreatesTodoAndExpectsToReturnAGuid()
        {
            //Given i have a todo
            var todo = new Todo { Name = "Fix pipe" };

            //And I have a timestampFacade
            var mockTimestampFacade = new Mock<ITimestampFacade>();

            // And I have a mock dict
            var mockTodos = new Dictionary<Guid, Todo>
            {
                { Guid.NewGuid(), new Todo { Id = Guid.NewGuid(), Name = "Clean bed" } },
                { Guid.NewGuid(), new Todo { Id = Guid.NewGuid(), Name = "some todo" } },
                { Guid.NewGuid(), new Todo { Id = Guid.NewGuid(), Name = "my todo" } },
            };

            //And I have a todo repo
            var todosRepository = new InMemoryTodosRepository(mockTodos, mockTimestampFacade.Object);
            mockTimestampFacade.Setup(s => s.GetTimestampInMilliseconds()).Returns(1625123333);

            //When I create this todo
            Guid actualTodoId = await todosRepository.CreateTodoAsync(todo);

            //Then I expect to return a guid
            Assert.True(actualTodoId != Guid.Empty);

            Todo actualTodo = mockTodos[actualTodoId];
            Assert.NotNull(actualTodo);
            Assert.Equal(mockTodos[actualTodoId].Id, actualTodoId);
            Assert.Equal(mockTodos[actualTodoId].Name, actualTodo.Name);
            Assert.Equal(mockTodos[actualTodoId].IsComplete, actualTodo.IsComplete);

            // Verify that expected/actual match timestamp
            Assert.Equal(mockTodos[actualTodoId].CreatedAt, actualTodo.CreatedAt);
            Assert.Equal(mockTodos[actualTodoId].UpdatedAt, actualTodo.UpdatedAt);

            // Verify that Create/Update math on same type
            Assert.Equal(mockTodos[actualTodoId].CreatedAt, mockTodos[actualTodoId].UpdatedAt);
            Assert.Equal(actualTodo.CreatedAt, actualTodo.UpdatedAt);
            
            // Verify dictionary count
            IEnumerable<Todo> actualTodoValues = mockTodos.Values;
            Assert.NotEmpty(actualTodoValues);
            Assert.Equal(4, actualTodoValues.Count());

            // Verify timestamp mock was called twice
            mockTimestampFacade.Verify(s => s.GetTimestampInMilliseconds(), Times.Exactly(2));
        }

        [Fact]
        public async Task TodosRepositoryUpdateTodoAsyncUpdatesTodoAndExpectsToReturnAGuid()
        {
            //Given i have a existing id
            Guid todoId = Guid.NewGuid();

            var expectedTodo = new Todo { Id = todoId, Name = "Fix pipe", IsComplete = true, CreatedAt = 123456, UpdatedAt = 123456 };

            //And I have a timestampFacade
            var mockTimestampFacade = new Mock<ITimestampFacade>();

            // And I have a mock dict
            var mockTodos = new Dictionary<Guid, Todo>
            {
                { Guid.NewGuid(), new Todo { Id = Guid.NewGuid(), Name = "Clean bed" } },
                { todoId, new Todo { Id = todoId, Name = "my todo", CreatedAt = 123456, UpdatedAt = 123456 } },
            };

            //And I have a todo repo
            var todosRepository = new InMemoryTodosRepository(mockTodos, mockTimestampFacade.Object);
            mockTimestampFacade.Setup(s => s.GetTimestampInMilliseconds()).Returns(1000000);

            //When I create this todo
            Guid actualTodoId = await todosRepository.UpdateTodoAsync(expectedTodo);

            //Then I expect to return a guid
            Assert.True(actualTodoId != Guid.Empty);

            Todo actualTodo = mockTodos[actualTodoId];
            Assert.NotNull(actualTodo);

            // Verify that expected/actual match timestamp
            Assert.Equal(expectedTodo.Id, actualTodo.Id);
            Assert.True(actualTodo.IsComplete);
            Assert.Equal(expectedTodo.CreatedAt, actualTodo.CreatedAt);
            Assert.True(expectedTodo.UpdatedAt < actualTodo.UpdatedAt);

            // Verify dictionary count
            IEnumerable<Todo> actualTodoValues = mockTodos.Values;
            Assert.NotEmpty(actualTodoValues);
            Assert.Equal(2, actualTodoValues.Count());

            // Verify timestamp mock was called twice
            mockTimestampFacade.Verify(s => s.GetTimestampInMilliseconds(), Times.Once);
        }
    }
}