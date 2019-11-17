using Blazored.LocalStorage;
using JE.App.Pages.Movie.List;
using JE.App.State;
using JE.Infrastructure.Services;
using Microsoft.AspNetCore.Components;
using Moq;
using ReduxSimple;

namespace JE.App.Tests.Pages.Movie.List.MovieListViewModelTests
{
    public abstract class MovieListViewModelTestsBase
    {
        protected MovieListViewModelTestsBase()
        {
            var mockRepository = new MockRepository(MockBehavior.Default);

            NavigationManagerMock = mockRepository.Create<NavigationManager>();
            OmdbMovieServiceMock = mockRepository.Create<IOmdbMovieService>();
            LocalStorageServiceMock = mockRepository.Create<ILocalStorageService>();
            MovieSearchStoreMock = mockRepository.Create<ReduxStore<MovieSearchState>>();
        }

        protected Mock<NavigationManager> NavigationManagerMock { get; }

        protected Mock<IOmdbMovieService> OmdbMovieServiceMock { get; }

        protected Mock<ILocalStorageService> LocalStorageServiceMock { get; }

        protected Mock<ReduxStore<MovieSearchState>> MovieSearchStoreMock { get; }

        protected MovieListViewModel CreateClass() =>
            new MovieListViewModel(
                OmdbMovieServiceMock.Object,
                NavigationManagerMock.Object,
                LocalStorageServiceMock.Object,
                MovieSearchStoreMock.Object);
    }
}
