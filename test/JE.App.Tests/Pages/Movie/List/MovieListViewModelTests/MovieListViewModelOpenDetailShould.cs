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

            NavigationManagerMock
                .Verify(x => x.NavigateTo($"/movie/{id}", false), Times.Once);
        }
    }
}
