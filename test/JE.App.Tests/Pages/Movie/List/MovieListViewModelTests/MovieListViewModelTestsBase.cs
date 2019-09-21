using Blazored.LocalStorage;
using JE.App.Pages.Movie.List;
using JE.App.State;
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

            NavigationManagerMock = mockRepository.Create<NavigationManager>();
            OmdbMovieServiceMock = mockRepository.Create<IOmdbMovieService>();
            LocalStorageServiceMock = mockRepository.Create<ILocalStorageService>();
        }

        protected Mock<NavigationManager> NavigationManagerMock { get; }

        protected Mock<IOmdbMovieService> OmdbMovieServiceMock { get; }

        protected Mock<ILocalStorageService> LocalStorageServiceMock { get; }

        protected MovieListViewModel CreateClass() =>
            new MovieListViewModel(
                OmdbMovieServiceMock.Object,
                NavigationManagerMock.Object,
                LocalStorageServiceMock.Object,
                new MovieSearchStore());
    }
}
