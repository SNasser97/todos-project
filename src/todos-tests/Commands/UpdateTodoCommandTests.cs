using System;
using System.Threading.Tasks;
using Moq;
using todos_data;
using todos_data.Repository;
using todos_logic.Todos.Command;
using todos_tests.Utility;
using Xunit;

namespace todos_tests.Commands
{
    public class UpdateTodoCommandTests
    {
        [Fact]
        public async Task TodosCommandUpdateTodosAsyncTakesTodoCommandObjectAndReturnsAGuid()
        {
            //  Given I have a UpdateTodoCommand
            var todo = new UpdateTodoCommand
            {
                Id = Guid.NewGuid(),
                Name = "Todo I must update",
                IsComplete = true
            };

            // And I have a mock todo repo
            var mockTodos = new Mock<ITodosRepository>();

            // And I havea  todos command
            var todosCommand = new TodosCommand(mockTodos.Object);
            mockTodos.Setup(s => s.GetTodoAsync(It.IsAny<Guid>())).ReturnsAsync(new Todo { Id = todo.Id, Name = "My old todo", IsComplete = false });
            mockTodos.Setup(s => s.UpdateTodoAsync(It.IsAny<Todo>())).ReturnsAsync(todo.Id);

            // When I provide the update command
            Guid actualId = await todosCommand.UpdateTodoAsync(todo);

            // Then I expect a guid to return
            Assert.True(actualId != Guid.Empty);
            Assert.Equal(todo.Id, actualId);

            mockTodos.Verify(s => s.GetTodoAsync(It.IsAny<Guid>()), Times.Once);
            mockTodos.Verify(s => s.UpdateTodoAsync(It.IsAny<Todo>()), Times.Once);
        }

        [Fact]
        public async Task TodosCommandUpdateTodosAsyncTakesNullUpdateCommandAndExpectsToThrowArgumentNullException()
        {
            // Given I have a null update command
            // And I have mock todos repo
            var mockTodos = new Mock<ITodosRepository>();

            // And I have a todosCommand
            var todosCommand = new TodosCommand(mockTodos.Object);

            // When I provide an empty update todo command
            // Then I expect to throw an ArgumentNullException
            await Exceptions.HandleExceptionsAsync<ArgumentNullException>(async () =>
                await todosCommand.UpdateTodoAsync(null),
                (ex) => Assert.Equal("todo", ex.ParamName)
            );
        }

        [Fact]
        public async Task TodosCommandUpdateTodosAsyncTakeEmptyGuidInUpdateCommandAndExpectsExceptionMessageIdIsEmpty()
        {
            // Given I have a null update command
            // And I have mock todos repo
            var mockTodos = new Mock<ITodosRepository>();

            // And I have a todosCommand
            var todosCommand = new TodosCommand(mockTodos.Object);

            // When I provide an empty update todo command
            // Then I expect to throw an ArgumentNullException
            await Exceptions.HandleExceptionsAsync<Exception>(async () =>
                await todosCommand.UpdateTodoAsync(new UpdateTodoCommand { Id = Guid.Empty, Name = "My todo", IsComplete = true}),
                (ex) => Assert.Equal("todo Id is empty", ex.Message)
            );
        }

        [Fact]
        public async Task TodosCommandUpdateTodosAsyncTakeEmptyNameInUpdateCommandAndExpectsExceptionMessageTodoNameIsEmpty()
        {
            // Given I have a null update command
            // And I have mock todos repo
            var mockTodos = new Mock<ITodosRepository>();

            // And I have a todosCommand
            var todosCommand = new TodosCommand(mockTodos.Object);

            // When I provide an empty update todo command
            // Then I expect to throw an ArgumentNullException
            await Exceptions.HandleExceptionsAsync<Exception>(async () =>
                await todosCommand.UpdateTodoAsync(new UpdateTodoCommand { Id = Guid.NewGuid(), Name = "", IsComplete = true}),
                (ex) => Assert.Equal("todo name was empty", ex.Message)
            );
        }
    }
}