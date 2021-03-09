using System;
using System.Threading.Tasks;
using Moq;
using todos_data;
using todos_data.Repository;
using todos_logic;
using todos_logic.Todos.Command;
using todos_tests.Utility;
using Xunit;

namespace todos_tests.Commands
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
            mockTodosRepository.Setup(s => s.GetTodoAsync(todoId)).ReturnsAsync(new Todo());
            mockTodosRepository.Setup(s => s.DeleteTodoAsync(todoId));

            //  When I provide this todo id
            await todosCommand.DeleteTodoAsync(todoId);

            //  Then I expect to delete the associated todo
            mockTodosRepository.Verify(s => s.GetTodoAsync(It.IsAny<Guid>()), Times.Once);
            mockTodosRepository.Verify(s => s.DeleteTodoAsync(It.IsAny<Guid>()), Times.Once);
        }
    }
}