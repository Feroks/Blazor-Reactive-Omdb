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
                // Ignore initial value of Id, which is default(string) (null)                
                .Skip(1)
                // It is an input parameter it is set only once, therefore use Take(1)
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
