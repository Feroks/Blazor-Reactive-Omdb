using Blazored.LocalStorage;
using JE.App.Pages.Movie.List;
using JE.Infrastructure.Services;
using Microsoft.AspNetCore.Components;
using Moq;

namespace JE.App.Tests.Pages.Movie.List.MovieListViewModelTests
{
    public abstract class MovieListViewModelTestsBase
    {
        protected MovieListViewModelTestsBase()
        {
            var mockRepository = new MockRepository(MockBehavior.Default);

            UriHelperMock = mockRepository.Create<IUriHelper>();
            OmdbMovieServiceMock = mockRepository.Create<IOmdbMovieService>();
            LocalStorageServiceMock = mockRepository.Create<ILocalStorageService>();
        }

        protected Mock<IUriHelper> UriHelperMock { get; }

        protected Mock<IOmdbMovieService> OmdbMovieServiceMock { get; }

        protected Mock<ILocalStorageService> LocalStorageServiceMock { get; }

        protected MovieListViewModel CreateClass() =>
            new MovieListViewModel(
                OmdbMovieServiceMock.Object,
                UriHelperMock.Object,
                LocalStorageServiceMock.Object);
    }
}
