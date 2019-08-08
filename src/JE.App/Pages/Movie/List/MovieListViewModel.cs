using DynamicData;
using DynamicData.Binding;
using JE.Core.Dto;
using JE.Infrastructure.Services;
using Microsoft.AspNetCore.Components;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;

namespace JE.App.Pages.Movie.List
{
    public class MovieListViewModel : BaseViewModel
    {
        private readonly IUriHelper _uriHelper;

        public MovieListViewModel(IOmdbMovieService omdbMovieService, IUriHelper uriHelper)
        {
            _uriHelper = uriHelper;

            var source = new SourceCache<OmdbMovieSearchDto, string>(x => x.ImdbId)
                .DisposeWith(CleanUp);

            source.Connect()
                .Sort(SortExpressionComparer<OmdbMovieSearchDto>.Ascending(p => p.Title), SortOptimisations.ComparesImmutableValuesOnly)
                .Bind(Movies)
                .Subscribe(_ => UpdateState())
                .DisposeWith(CleanUp);

            this.WhenAnyValue(x => x.SearchText)
                .Skip(1)
                .Throttle(TimeSpan.FromMilliseconds(250))
                .SelectMany(omdbMovieService.SearchAsync)
                .Subscribe(x => source.Edit(list =>
                {
                    list.Clear();

                    if (x != null)
                        list.AddOrUpdate(x);
                }))
                .DisposeWith(CleanUp);
        }

        [Reactive]
        public string SearchText { get; set; }

        public IObservableCollection<OmdbMovieSearchDto> Movies { get; } = new ObservableCollectionExtended<OmdbMovieSearchDto>();

        public void OpenDetail(string id) => _uriHelper.NavigateTo($"/movie/{id}");
    }
}
