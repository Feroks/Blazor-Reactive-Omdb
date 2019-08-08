using System;
using JE.Infrastructure.Services;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System.Reactive.Disposables;
using System.Reactive.Linq;

namespace JE.App.Pages.Movie.List
{
    public class MovieListViewModel : BaseViewModel
    {
        [Reactive]
        public string SearchText { get; set; }

        public MovieListViewModel(IOmdbMovieService omdbMovieService)
        {
            this.WhenAnyValue(x => x.SearchText)
                .Skip(1)
                .SelectMany(omdbMovieService.SearchAsync)
                .Subscribe()
                .DisposeWith(CleanUp);
        }
    }
}
