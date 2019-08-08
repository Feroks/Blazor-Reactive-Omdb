using System;
using JE.Infrastructure.Services;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using DynamicData;
using DynamicData.Binding;
using JE.Core.Dto;

namespace JE.App.Pages.Movie.List
{
    public class MovieListViewModel : BaseViewModel
    {
        public MovieListViewModel(IOmdbMovieService omdbMovieService)
        {
            var source = new SourceCache<OmdbMovieSearchDto, string>(x => x.ImdbId)
                .DisposeWith(CleanUp);

            source.Connect()
                .Sort(SortExpressionComparer<OmdbMovieSearchDto>.Ascending(p => p.Title), SortOptimisations.ComparesImmutableValuesOnly)
                .Bind(Movies)
                .Subscribe()
                .DisposeWith(CleanUp);

            this.WhenAnyValue(x => x.SearchText)
                .Skip(1)
                .SelectMany(omdbMovieService.SearchAsync)
                .Subscribe(x => source.Edit(list =>
                {
                    list.Clear();
                    list.AddOrUpdate(x);
                }))
                .DisposeWith(CleanUp);
        }

        [Reactive]
        public string SearchText { get; set; }

        public IObservableCollection<OmdbMovieSearchDto> Movies { get; } = new ObservableCollectionExtended<OmdbMovieSearchDto>();
    }
}
