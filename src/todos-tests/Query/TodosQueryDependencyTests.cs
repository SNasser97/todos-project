using System;
using todos_logic.Todos.Query;
using todos_tests.Utility;
using Xunit;

namespace todos_tests.Query
{
    public class TodosQueryDependencyTests
    {
        [Fact]
        public void TodosQueryTakesNullTodosRepositoryAndExpectsToThrowArgumentNullException()
        {
            // Given I have a TodosQuery instance
            // When provide a null dependency
            // Then I expect to throw Argument Null Exception
            Exceptions.HandleExceptions<ArgumentNullException>(() =>
                new TodosQuery(null),
                (ex) => Assert.Equal("todosRepository", ex.ParamName)
            );
        }
    }
}