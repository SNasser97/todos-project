using System;
using System.Threading.Tasks;
using Moq;
using todos_data;
using todos_data.Repository;
using todos_logic.Todos.Query;
using todos_tests.Utility;
using Xunit;

namespace todos_tests.Query
{
    public class GetTodoQueryTests
    {
        [Fact]
        public async Task TodosQueryGetTodoAsyncTakesGuidAndExpectsToReturnATodo()
        {
            //  Given I have a guid
            var todoId = Guid.NewGuid();

            // And I have an expectedTodo
            var expectedTodo = new Todo { Id = todoId, Name = "My existing todo", IsComplete = true, CreatedAt = 1234523236, UpdatedAt = 1434564236 };

            // And I have a mock TodoRepo
            var mockTodoRepo = new Mock<ITodosRepository>();

            // And I have an instance of todosQuery
            var todosQuery = new TodosQuery(mockTodoRepo.Object);
            mockTodoRepo.Setup(s => s.GetTodoAsync(It.IsAny<Guid>())).ReturnsAsync(expectedTodo);

            //  When I provide this guid
            Todo actualTodo = await todosQuery.GetTodoAsync(todoId);

            //  Then I expect a Todo to be returned
            Assert.NotNull(actualTodo);
            Assert.Equal(expectedTodo, actualTodo);
            Assert.Equal(expectedTodo.Id, actualTodo.Id);
            Assert.Equal(expectedTodo.Name, actualTodo.Name);
            Assert.Equal(expectedTodo.IsComplete, actualTodo.IsComplete);
            Assert.Equal(expectedTodo.CreatedAt, actualTodo.CreatedAt);
            Assert.Equal(expectedTodo.UpdatedAt, actualTodo.UpdatedAt);

            mockTodoRepo.Verify(s => s.GetTodoAsync(It.IsAny<Guid>()), Times.Once);
        }

        [Fact]
        public async Task TodosQueryGetTodoAsyncTakesGuidAndExpectsExceptionMessageTodoNotFound()
        {
            //  Given I have a guid
            var todoId = Guid.NewGuid();

            // And I have a mock TodoRepo
            var mockTodoRepo = new Mock<ITodosRepository>();

            // And I have an instance of todosQuery
            var todosQuery = new TodosQuery(mockTodoRepo.Object);
            mockTodoRepo.Setup(s => s.GetTodoAsync(It.IsAny<Guid>())).ReturnsAsync(null as Todo);

            //  When I provide this guid
            await Exceptions.HandleExceptionsAsync<Exception>(async () =>
                await todosQuery.GetTodoAsync(todoId),
                (ex) => Assert.Equal("todo not found", ex.Message)
            );

            //  Then I expect an Exception todo not found
            mockTodoRepo.Verify(s => s.GetTodoAsync(It.IsAny<Guid>()), Times.Once);
        }

        [Fact]
        public async Task TodosQueryGetTodoAsyncTakesEmptyGuidAndExpectsArgumentNullException()
        {
            //  Given I have a guid
            Guid emptyGuid = Guid.Empty;

            // And I have a mock TodoRepo
            var mockTodoRepo = new Mock<ITodosRepository>();

            // And I have an instance of todosQuery
            var todosQuery = new TodosQuery(mockTodoRepo.Object);

            //  When I provide this guid
            //  Then I expect an ArgumentNullException
            await Exceptions.HandleExceptionsAsync<ArgumentNullException>(async () =>
                await todosQuery.GetTodoAsync(emptyGuid),
                (ex) => Assert.Equal("id", ex.ParamName)
            );
        }
    }
}