using FluentAssertions;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace JE.App.Tests.Pages.Movie.List.MovieListViewModelTests
{
    public class MovieListViewModelGetSearchTextValuesAsyncShould : MovieListViewModelTestsBase
    {
        [Fact]
        public async Task ReturnEmptyEnumerableIfNoSearchTextValuesPresent()
        {
            var vm = CreateClass();

            var result = await vm.GetSearchTextValuesAsync();

            result
                .Should()
                .NotBeNull()
                .And
                .BeEquivalentTo(Enumerable.Empty<string>());
        }
    }
}
