﻿using Blazored.LocalStorage;
using DynamicData;
using DynamicData.Binding;
using JE.Core.Dto;
using JE.Infrastructure.Services;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Components;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

[assembly: InternalsVisibleTo("JE.App.Tests")]
namespace JE.App.Pages.Movie.List
{
    public class MovieListViewModel : BaseViewModel
    {
        private const string SearchTextKey = "MovieSearchTextKey";
        private readonly IUriHelper _uriHelper;
        private readonly ILocalStorageService _localStorageService;

        public MovieListViewModel(IOmdbMovieService omdbMovieService, IUriHelper uriHelper, ILocalStorageService localStorageService)
        {
            _uriHelper = uriHelper;
            _localStorageService = localStorageService;

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
                // Skip initial value which is null
                .Skip(1)
                // Use throttle to prevent over requesting data
                .Throttle(TimeSpan.FromMilliseconds(250))
                .Publish();

            searchTextObservable
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

            searchTextObservable
                .SelectMany(async x =>
                {
                    await UpdateSearchTextsAsync(x);

                    return Unit.Default;
                })
                .Subscribe()
                .DisposeWith(CleanUp);

            searchTextObservable.Connect();
        }

        [Reactive]
        public string SearchText { get; set; }

        [Reactive]
        public bool IsSearching { get; set; }

        [UsedImplicitly]
        public bool IsSourceEmpty { [ObservableAsProperty] get; }

        public IObservableCollection<OmdbMovieSearchDto> Movies { get; } = new ObservableCollectionExtended<OmdbMovieSearchDto>();

        public void OpenDetail(string id) => _uriHelper.NavigateTo($"/movie/{id}");

        internal async Task UpdateSearchTextsAsync(string x)
        {
            var searchedTexts = await GetSearchTextValuesAsync();

            var newTexts = searchedTexts
                .TakeLast(4)
                .Append(x);

            await _localStorageService
                .SetItemAsync(SearchTextKey, newTexts)
                .ConfigureAwait(false);
        }

        [ItemNotNull]
        internal async Task<IEnumerable<string>> GetSearchTextValuesAsync()
        {
            var searchedTexts = await _localStorageService
                .GetItemAsync<string[]>(SearchTextKey)
                .ConfigureAwait(false);

            return searchedTexts ?? Enumerable.Empty<string>();
        }
    }
}
