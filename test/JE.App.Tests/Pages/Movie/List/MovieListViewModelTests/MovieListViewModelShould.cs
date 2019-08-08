using Moq;
using System.Threading.Tasks;
using Xunit;

namespace JE.App.Tests.Pages.Movie.List.MovieListViewModelTests
{
    public class MovieListViewModelShould : MovieListViewModelTestsBase
    {
        [Fact]
        public async Task PerformSearchWhenSearchTextChanges()
        {
            var vm = CreateClass();

            const string searchText = "star wars";

            vm.SearchText = searchText;

            await Task.Delay(500);

            OmdbMovieServiceMock
                .Verify(x => x.SearchAsync(searchText), Times.Once);
        }
    }
}
