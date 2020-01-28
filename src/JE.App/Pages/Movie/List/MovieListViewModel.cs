using Blazored.LocalStorage;
using DynamicData;
using DynamicData.Binding;
using JE.App.State;
using JE.Core.Dto;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Components;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using ReduxSimple;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace JE.App.Pages.Movie.List
{
    public class MovieListViewModel : BaseViewModel
    {
        private const string SearchTextKey = "MovieSearchTextKey";
        private readonly NavigationManager _navigationManager;
        private readonly ILocalStorageService _localStorageService;

        public MovieListViewModel(
            NavigationManager navigationManager,
            ILocalStorageService localStorageService,
            ReduxStore<MovieSearchState> movieSearchStore)
        {
            _navigationManager = navigationManager;
            _localStorageService = localStorageService;
            
            // Set initial value
            SearchText = movieSearchStore.State.SearchText;

            var source = new SourceCache<OmdbMovieSearchDto, string>(x => x.ImdbId)
                .DisposeWith(CleanUp);

            source.Connect()
                .Sort(SortExpressionComparer<OmdbMovieSearchDto>.Ascending(p => p.Title), SortOptimisations.ComparesImmutableValuesOnly)
                .Bind(Movies)
                .Subscribe(_ => UpdateState())
                .DisposeWith(CleanUp);

            source.CountChanged
                .StartWith(0)
                .Select(x => x == 0)
                .ToPropertyEx(this, x => x.IsSourceEmpty)
                .DisposeWith(CleanUp);

            var searchTextObservable = this.WhenAnyValue(x => x.SearchText)
                .Skip(1)
                // Use throttle to prevent over requesting data
                .Throttle(TimeSpan.FromMilliseconds(250))
                .Publish();

            searchTextObservable
                .Where(string.IsNullOrEmpty)
                .Subscribe(_ => movieSearchStore.Dispatch(new ResetMovieSearchAction()))
                .DisposeWith(CleanUp);
                
            searchTextObservable
                .Where(x => !string.IsNullOrEmpty(x))
                .Subscribe(x => movieSearchStore.Dispatch(new PerformMovieSearchAction(x)))
                .DisposeWith(CleanUp);

            searchTextObservable.Connect();
            
            movieSearchStore
                .Select(MovieSearchSelectors.SelectIsSearching)
                .ToPropertyEx(this, x => x.IsSearching)
                .DisposeWith(CleanUp);
            
            movieSearchStore
                .Select(MovieSearchSelectors.SelectMovies)
                .Subscribe(x => source.Edit(list =>
                {
                    list.Clear();
                    list.AddOrUpdate(x);
                }))
                .DisposeWith(CleanUp);
            
            movieSearchStore
                .Select(MovieSearchSelectors.SelectSearchText)
                .Skip(1)
                .SelectMany(async x =>
                {
                    await SaveSearchTextsAsync(x)
                        .ConfigureAwait(false);

                    return Unit.Default;
                })
                .Subscribe()
                .DisposeWith(CleanUp);
        }

        [Reactive]
        public string SearchText { get; set; }

        [UsedImplicitly]
        public bool IsSearching { [ObservableAsProperty] get; }

        [UsedImplicitly]
        public bool IsSourceEmpty { [ObservableAsProperty] get; }

        public IObservableCollection<OmdbMovieSearchDto> Movies { get; } = new ObservableCollectionExtended<OmdbMovieSearchDto>();

        public void OpenDetail(string id) => _navigationManager.NavigateTo($"/movie/{id}");
        
        /// <summary>
        /// Save Search Text in Local storage. Only last 5 searches are saved
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        private async Task SaveSearchTextsAsync(string x)
        {
            var searchedTexts = await LoadSearchTextValuesAsync()
                .ConfigureAwait(false);

            var newTexts = searchedTexts
                .TakeLast(4)
                .Append(x);

            await _localStorageService
                .SetItemAsync(SearchTextKey, newTexts)
                .ConfigureAwait(false);
        }

        /// <summary>
        /// Load Last Search Texts stored in Local storage
        /// </summary>
        /// <returns></returns>
        [ItemNotNull]
        internal async Task<IEnumerable<string>> LoadSearchTextValuesAsync()
        {
            var searchedTexts = await _localStorageService
                .GetItemAsync<string[]>(SearchTextKey)
                .ConfigureAwait(false);

            return searchedTexts ?? Enumerable.Empty<string>();
        }
    }
}
