using DynamicData;
using DynamicData.Binding;
using JE.Core.Dto;
using JE.Infrastructure.Services;
using JetBrains.Annotations;
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
                .Do(_ => IsSearching = true)
                .SelectMany(omdbMovieService.SearchAsync)
                .Do(_ => IsSearching = false)
                .Subscribe(x => source.Edit(list =>
                {
                    list.Clear();

                    if (x != null)
                        list.AddOrUpdate(x);
                }))
                .DisposeWith(CleanUp);

            source.CountChanged
                .StartWith(0)
                .Select(x => x == 0)
                .ToPropertyEx(this, x => x.IsSourceEmpty)
                .DisposeWith(CleanUp);
        }

        [Reactive]
        public string SearchText { get; set; }
        
        [UsedImplicitly] 
        public bool IsSourceEmpty { [ObservableAsProperty] get;  }

        [Reactive]
        public bool IsSearching { get; set; }

        public IObservableCollection<OmdbMovieSearchDto> Movies { get; } = new ObservableCollectionExtended<OmdbMovieSearchDto>();

        public void OpenDetail(string id) => _uriHelper.NavigateTo($"/movie/{id}");
    }
}
