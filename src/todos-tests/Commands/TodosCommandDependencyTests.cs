using System;
using todos_logic.Todos.Command;
using todos_tests.Utility;
using Xunit;

namespace todos_tests.Commands
{
    public class TodosCommandDependencyTests
    {
        [Fact]
        public void TodosCommandTakesNullTodosRepositoryDependencyAndExpectsToThrowArgumentNullException()
        {
            //  Given I have a null dependency
            //  When I provide null type
            //  Then I expect to throw ArgumentNullException
            Exceptions.HandleExceptions<ArgumentNullException>(() =>
                new TodosCommand(null),
                (ex) => Assert.Equal("todosRepository", ex.ParamName)
            );
        }
    }
}