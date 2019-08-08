using System.Collections.Generic;
using Moq;
using System.Threading.Tasks;
using FluentAssertions;
using JE.Core.Dto;
using Xunit;

namespace JE.App.Tests.Pages.Movie.List.MovieListViewModelTests
{
    public class MovieListViewModelShould : MovieListViewModelTestsBase
    {
        [Fact]
        public async Task IgnoreInitialSearchTextValue()
        {
            CreateClass();

            // Because of Delay
            await Task.Delay(500);

            OmdbMovieServiceMock
                .Verify(x => x.SearchAsync(It.IsAny<string>()), Times.Never);
        }

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

        [Fact]
        public async Task SetIsSearchingValueWhenPerformingSearch()
        {
            OmdbMovieServiceMock
                .Setup(x => x.SearchAsync(It.IsAny<string>()))
                .Returns(async () =>
                {
                    await Task.Delay(200);
                    return new List<OmdbMovieSearchDto>();
                });

            var vm = CreateClass();

            const string searchText = "star wars";

            vm.SearchText = searchText;

            await Task.Delay(300);
            vm.IsSearching
                .Should()
                .BeTrue();

            await Task.Delay(200);
            vm.IsSearching
                .Should()
                .BeFalse();

            OmdbMovieServiceMock
                .Verify(x => x.SearchAsync(searchText), Times.Once);
        }
    }
}
