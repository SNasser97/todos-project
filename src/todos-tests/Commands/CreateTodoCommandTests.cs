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
    public class CreateTodoCommandTests
    {
        [Fact]
        public async Task TodosCommandCreateTodosAsyncTakesNullCommandAndExpectsToThrowArgumentNullException()
        {
            //Given I have a null create command
            //And I have a mock todo repo
            var mockUserRepository = new Mock<ITodosRepository>();

            //And I have a todos command
            var todosCommand = new TodosCommand(mockUserRepository.Object);

            //When I provide a null create command
            //Then I expect to throw arg null ex
            await Exceptions.HandleExceptionsAsync<ArgumentNullException>(async () =>
                await todosCommand.CreateTodoAsync(null as CreateTodoCommand),
                (ex) => Assert.Equal("todo", ex.ParamName)
            );
        }

        [Fact]
        public async Task TodosCommandCreateTodosAsyncTakesCreateTodoCommandAndReturnsAGuid()
        {
            // Given I have a CreateTodoCommand
            var newTodo = new CreateTodoCommand { Name = "My todo" };
            
            // And I have an expected Id
            Guid expectedId = Guid.NewGuid();

            // And I have a mock todo repo
            var mockTodosRepository = new Mock<ITodosRepository>();

            // And I have a todosCommand instance
            var todosCommand = new TodosCommand(mockTodosRepository.Object);
            mockTodosRepository.Setup(s => s.CreateTodoAsync(It.IsAny<Todo>())).ReturnsAsync(expectedId);
            
            // When I provide the CreateTodoCommand
            Guid actualTodoId = await todosCommand.CreateTodoAsync(newTodo);

            // Then I expect a Guid to return
            Assert.True(actualTodoId != Guid.Empty);
            Assert.Equal(expectedId, actualTodoId);

            // Verify assertions
            mockTodosRepository.Verify(s => s.CreateTodoAsync(It.IsAny<Todo>()), Times.Once);
        }

        [Fact]
        public async Task TodosCommandCreateTodosAsyncTakesEmptyCreateTodoCommandExpectsToThrowArgumentNullException()
        {
            // Given I have a mock todos repo
            var mockTodosRepository = new Mock<ITodosRepository>();

            // And I have an instance of todos command
            var todosCommand = new TodosCommand(mockTodosRepository.Object);

            // When I provide an empty CreateTodoCommand
            // Then I expect to throw ArgumentNulLException
            await Exceptions.HandleExceptionsAsync<ArgumentNullException>(async () => 
                await todosCommand.CreateTodoAsync(null),
                (ex) => Assert.Equal("todo", ex.ParamName)
            );
        }

        [Fact]
        public async Task TodosCommandCreateTodosAsyncTakesCreateTodoCommandWithEmptyNameAndExpectsToThrowAnException()
        {
            // Given I have a CreateCommandTodo
            var newTodo = new CreateTodoCommand();

            //  And I have a mock todos repo
            var mockTodosRepository = new Mock<ITodosRepository>();

            // And I have a todos command
            var todosCommand = new TodosCommand(mockTodosRepository.Object);

            //  When I provide this CreateCommandTodo
            //  Then I expect an exception
            await Exceptions.HandleExceptionsAsync<Exception>(async () => 
                await todosCommand.CreateTodoAsync(newTodo),
                (ex) => Assert.Equal("todo name was empty", ex.Message)
            );
        }
    }
}