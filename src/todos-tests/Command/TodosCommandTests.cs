using System;
using System.Threading.Tasks;
using Moq;
using todos_data.Repository;
using todos_logic;
using todos_logic.Todos.Command;
using todos_tests.Utility;
using Xunit;

namespace todos_tests.Command
{
    public class TodosCommandTests
    {
        // [Fact]
        // public void TodosCommandTakesNullDependencyAndExpectsToThrowArgumentNullException()
        // {
        //     // Given I have an empty dependency
        //     // And I have an instance of todosCommand
        //     // When I provide this empty dependency
        //     // Then I expect to throw arg null exeption
        //     Exceptions.HandleExceptions<ArgumentNullException>(() => 
        //         new TodosCommand(null),
        //         (ex) => Assert.Equal("todosRepository", ex.ParamName)
        //     );
        // }

        // [Fact]
        // public async Task TodosCommandCreateTodosAsyncTakesNullCommandAndExpectsToThrowArgumentNullException()
        // {
        //     //Given I have a null create command
        //     //And I have a mock todo repo
        //     var mockUserRepository = new Mock<ITodosRepository>();

        //     //And I have a todos command
        //     var todosCommand = new TodosCommand(mockUserRepository.Object);

        //     //When I provide a null create command
        //     await Exceptions.HandleExceptionsAsync<ArgumentNullException>(async () =>
        //         await todosCommand.CreateTodoAsync(null as CreateTodoCommand),
        //         (ex) => Assert.Equal("todo", ex.ParamName)
        //     );

        //     //Then I expect to throw arg null ex
        // }
    }
}