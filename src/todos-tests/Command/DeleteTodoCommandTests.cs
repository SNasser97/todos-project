using System;
using System.Threading.Tasks;
using Moq;
using todos_data;
using todos_data.Repository;
using todos_logic;
using todos_logic.Todos.Command;
using todos_tests.Utility;
using Xunit;

namespace todos_tests.Command
{
    public class DeleteTodoCommandTests
    {
        [Fact]
        public async Task TodosCommandDeleteTodosAsyncTakesGuidAndExpectsToDeleteTodo()
        {
            //  Given I have a todo id
            Guid todoId = Guid.NewGuid();        

            // And I have a mock todos repo
            var mockTodosRepository = new Mock<ITodosRepository>();

            // And I have a todos command
            var todosCommand = new TodosCommand(mockTodosRepository.Object);
            mockTodosRepository.Setup(s => s.GetTodoAsync(todoId)).ReturnsAsync(new Todo { Name = "My Todo" });
            mockTodosRepository.Setup(s => s.DeleteTodoAsync(todoId));

            //  When I provide this todo id
            await todosCommand.DeleteTodoAsync(todoId);

            //  Then I expect to delete the associated todo
            mockTodosRepository.Verify(s => s.GetTodoAsync(It.IsAny<Guid>()), Times.Once);
            mockTodosRepository.Verify(s => s.DeleteTodoAsync(It.IsAny<Guid>()), Times.Once);
        }

        [Fact]
        public async Task TodosCommandDeleteTodosAsyncTakesEmptyGuidAndExpectsToThrowArgumentNullException()
        {
            //  Given I have an empty id
            // And I have a mock todos repo
            var mockTodosRepository = new Mock<ITodosRepository>();

            // And I have a todos command
            var todosCommand = new TodosCommand(mockTodosRepository.Object);
            
            //  When I provide an empty id
            //  Then I expect to throw an ArgumentNullException
            await Exceptions.HandleExceptionsAsync<ArgumentNullException>(async () =>
                await todosCommand.DeleteTodoAsync(Guid.Empty),
                (ex) => Assert.Equal("id", ex.ParamName)
            );
        }

        [Fact]
        public async Task TodosCommandDeleteTodosAsyncTakesGuidAndExpectsExceptionMessageTodoNotFound()
        {
            // Given I have a todo id
            Guid todoId = Guid.NewGuid();

            // And I have a mock todos repo
            var mockTodosRepository = new Mock<ITodosRepository>();

            // And I have a todos command
            var todosCommand = new TodosCommand(mockTodosRepository.Object);
            mockTodosRepository.Setup(s => s.GetTodoAsync(todoId)).ReturnsAsync(null as Todo);

            // When I provide this todoId
            // Then I expect an exception of Todo Not Found
            await Exceptions.HandleExceptionsAsync<Exception>(async () => 
                await todosCommand.DeleteTodoAsync(todoId),
                (ex) => Assert.Equal("todo not found", ex.Message)
            );

            mockTodosRepository.Verify(s => s.GetTodoAsync(It.IsAny<Guid>()), Times.Once);
            mockTodosRepository.Verify(s => s.DeleteTodoAsync(It.IsAny<Guid>()), Times.Never);
        }
    }
}