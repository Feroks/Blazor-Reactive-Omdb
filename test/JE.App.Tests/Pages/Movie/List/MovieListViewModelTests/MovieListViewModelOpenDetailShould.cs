using Moq;
using Xunit;

namespace JE.App.Tests.Pages.Movie.List.MovieListViewModelTests
{
    public class MovieListViewModelOpenDetailShould : MovieListViewModelTestsBase
    {
        [Fact]
        public void NavigateToDetailPage()
        {
            const string id = "my_movie_id";

            var vm = CreateClass();

            vm.OpenDetail(id);

            UriHelperMock
                .Verify(x => x.NavigateTo($"/movie/{id}"), Times.Once);
        }
    }
}
