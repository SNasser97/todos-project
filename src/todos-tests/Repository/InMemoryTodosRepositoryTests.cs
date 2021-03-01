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
        // * Positive tests
        [Fact]
        public void TodosRepositoryTakesNullDictionaryAndExpectsToThrowArgumentNullException()
        {
            // Given I have a todos repo
            // And I have a null todos dict dependency
            // When I inject this null todo dict
            // Then I expect to throw an argument null exception
            Exceptions.HandleExceptions<ArgumentNullException>(() =>
                new InMemoryTodosRepository(null as Dictionary<Guid, Todo>, new TimestampFacade()),
                (ex) => Assert.Equal("todos", ex.ParamName)
            );
        }

        [Fact]
        public void TodosRepositoryTakesNullTimestampFacadeAndExpectsToThrowArgumentNullException()
        {
            // Given I have a todos repo
            // And I have a null timestamp dependency
            // When I inject this null timestamp dependency
            // Then I expect to throw an argument null exception
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

        [Fact]
        public async Task TodosRepositoryDeleteTodosAsyncTakesGuidAndExpectsToDeleteTodo()
        {
            // Given I have a todo Id
            Guid todoId = Guid.NewGuid();
            
            // And I have a mock dict
            var mockTodos = new Dictionary<Guid, Todo>
            {
                { todoId, new Todo { Id = todoId, Name = "Clean dishes", CreatedAt = 12345, UpdatedAt = 12345 } },
                { Guid.NewGuid(), new Todo {  Name = "Clean dishes", CreatedAt = 12345, UpdatedAt = 12345 } },
                { Guid.NewGuid(), new Todo { Name = "Clean dishes", CreatedAt = 12345, UpdatedAt = 12345 } },
            };
            
            // And I have a TodoRepo
            var todosRepository = new InMemoryTodosRepository(mockTodos, new Mock<ITimestampFacade>().Object);

            // When I provide an id
            await todosRepository.DeleteTodoAsync(todoId);

            // Then I expect to delete the todo associated
            Assert.Equal(2, mockTodos.Values.Count);

            // Verify that correct item not found
            bool actualTodoIsEmpty = mockTodos.Values.Any(t => t.Id != todoId);
            Assert.True(actualTodoIsEmpty);

            // Verify value is null
            Todo actualTodo = mockTodos.Values.FirstOrDefault(t => t.Id == todoId);
            Assert.Null(actualTodo);
        }

        [Fact]
        public async Task TodosRepositoryGetTodoAsyncTakesGuidAndExpectsToReturnATodo()
        {
            // Given I have an Id
            Guid expectedTodoId = Guid.NewGuid();
            
            Guid todoId = Guid.NewGuid();

            // And I have a mock dict
            var mockTodos = new Dictionary<Guid, Todo>
            {
                { expectedTodoId, new Todo { Id = expectedTodoId, Name = "My todo", CreatedAt = 1234, UpdatedAt = 1234, IsComplete = true } },
                { todoId , new Todo { Id = todoId, Name = "My  other todo", CreatedAt = 12334, UpdatedAt = 12334, IsComplete = false } },
            };

            // And I have a todo repo
            var todosRepository = new InMemoryTodosRepository(mockTodos, new TimestampFacade());

            // When I provide the expected todo Id
            Todo actualTodo = await todosRepository.GetTodoAsync(expectedTodoId);

            // Then I expect to return a todo
            Assert.NotNull(actualTodo);

            Assert.Equal(expectedTodoId, actualTodo.Id);
            Assert.Equal(mockTodos[expectedTodoId].Name, actualTodo.Name);
            Assert.Equal(mockTodos[expectedTodoId].CreatedAt, actualTodo.CreatedAt);
            Assert.Equal(mockTodos[expectedTodoId].UpdatedAt, actualTodo.UpdatedAt);
            Assert.Equal(mockTodos[expectedTodoId].IsComplete, actualTodo.IsComplete);
        }

        [Fact]
        public async Task TodosRepositoryGetTodosAsyncWhenInvokedExpectsToReturnsAListOfTodos()
        {
            // Given I have a mock todos
            var mockTodos = new Dictionary<Guid, Todo>
            {
                { Guid.NewGuid(), new Todo { Name = "my todo", CreatedAt = 1234, UpdatedAt = 12345, IsComplete = true } },
                { Guid.NewGuid(), new Todo { Name = "my todo1", CreatedAt = 567767, UpdatedAt = 567767, IsComplete = false } },
                { Guid.NewGuid(), new Todo { Name = "my todo2", CreatedAt = 3232, UpdatedAt = 3232, IsComplete = true } },
            };

            // And I have a todo repo

            var todosRepository = new InMemoryTodosRepository(mockTodos, new TimestampFacade());
            
            // When I call GetTodosAsync
            IEnumerable<Todo> actualTodosList = await todosRepository.GetTodosAsync();

            // Then I expect a list of todos
            Assert.NotEmpty(actualTodosList);

            Assert.Equal(3, actualTodosList.Count());

            foreach (Todo actualTodo in actualTodosList)
            {
                Todo expectedTodo = mockTodos.Values.FirstOrDefault(t => t.Id == actualTodo.Id);
                Assert.NotNull(expectedTodo);
                Assert.Equal(expectedTodo.Name, actualTodo.Name);
                Assert.Equal(expectedTodo.CreatedAt, actualTodo.CreatedAt);
                Assert.Equal(expectedTodo.UpdatedAt, actualTodo.UpdatedAt);
                Assert.Equal(expectedTodo.IsComplete, actualTodo.IsComplete);
            }
        }

        // ! Negative test
        [Fact]
        public async Task TodosRepositoryUpdateTodoAsyncTakesInvalidTodoAndExpectsToReturnAnEmptyGuid()
        {
            // Given I have a id
            Guid todoId = Guid.NewGuid();

            // And I have a invalid todo to updated
            Guid invalidId = Guid.NewGuid();
            var invalidTodo = new Todo { Id = invalidId, Name = "clean dishes", CreatedAt = 34354, UpdatedAt = 34354, IsComplete = true };

            // And I have a mock todo
            var mockTodos = new Dictionary<Guid, Todo>
            {
                { todoId, new Todo { Id = todoId, Name = "my todo", CreatedAt = 12345, UpdatedAt = 1234}}
            };
            
            // And I have a todo repo
            var todosRepository = new InMemoryTodosRepository(mockTodos, new TimestampFacade());

            // When I provide this invalidTodo
            Guid actualId = await todosRepository.UpdateTodoAsync(invalidTodo); 

            // Then I expect an empty guid
            Assert.True(actualId == Guid.Empty);
        }

        [Fact]
        public async Task TodosRepositoryGetTodosAsyncInvokesAndExpectsToReturnAnEmptyList()
        {
            // Given I a todo repo
            var todosRepository = new InMemoryTodosRepository(new Dictionary<Guid, Todo>(), new TimestampFacade());
            
            // When I call GetTodosAsync
            IEnumerable<Todo> actualTodos = await todosRepository.GetTodosAsync();

            // Then I expect to return an empty list
            Assert.Empty(actualTodos);
        }

        [Fact]
        public async Task TodosRepositoryGetTodoAsyncTakesInvalidGuidAndExpectsToReturnNull()
        {
            // Given I have mock todos
            var mockTodos = new Dictionary<Guid, Todo>
            {
                { Guid.NewGuid(), new Todo { Name = "my todo", CreatedAt = 1234, UpdatedAt = 1234} }
            };
            
            // And I have a todosRepo
            var todosRepository = new InMemoryTodosRepository(mockTodos, new TimestampFacade());

            // When I provide an invalid id
            Todo actualTodo = await todosRepository.GetTodoAsync(Guid.NewGuid());

            // Then  I expect to an empty todo
            Assert.Null(actualTodo);
        }

        [Fact]
        public async Task TodosRepositoryDeleteTodosAsyncTakesInvalidGuidAndExpectsToNotDeleteTodo()
        {
            // Given I have an todo id
            Guid todoId = Guid.NewGuid();

            // And I have mock todos
            var mockTodos = new Dictionary<Guid, Todo>
            {
                { Guid.NewGuid(), new Todo() },
                { Guid.NewGuid(), new Todo() },
                { Guid.NewGuid(), new Todo() },
            };
            
            // And I have a todo repo
            var todosRepository = new InMemoryTodosRepository(mockTodos, new TimestampFacade());

            // When I provide this id
            await todosRepository.DeleteTodoAsync(todoId);

            // Then I expect my mockTodo values to not be deleted
            Assert.NotEmpty(mockTodos);
            Assert.Equal(3, mockTodos.Values.Count);
        }
    }
}