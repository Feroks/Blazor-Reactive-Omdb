using JE.Core.Dto;
using JE.Infrastructure.Services;
using JetBrains.Annotations;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System.Reactive.Disposables;
using System.Reactive.Linq;

namespace JE.App.Pages.Movie.Detail
{
    public class MovieDetailViewModel : BaseViewModel
    {
        public MovieDetailViewModel(IOmdbMovieService omdbMovieService)
        {
            this.WhenAnyValue(x => x.Id)
                .Skip(1)
                .Take(1)
                .SelectMany(omdbMovieService.GetAsync)
                .ToPropertyEx(this, x => x.Movie)
                .DisposeWith(CleanUp);
        }

        [Reactive]
        public string Id { get; set; }

        [UsedImplicitly, CanBeNull]
        public OmdbMovieDto Movie { [ObservableAsProperty]get; }
    }
}
