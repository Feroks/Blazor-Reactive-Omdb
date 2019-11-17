using JE.Infrastructure.Services;
using ReduxSimple;
using System.Reactive.Linq;
using static ReduxSimple.Effects;

namespace JE.App.State
{
    public class MovieSearchEffects
    {
        private readonly ReduxStore<MovieSearchState> _store;
        private readonly IOmdbMovieService _omdbMovieService;

        public MovieSearchEffects(ReduxStore<MovieSearchState> store, IOmdbMovieService omdbMovieService)
        {
            _store = store;
            _omdbMovieService = omdbMovieService;
        }

        public Effect<MovieSearchState> SearchMovies => CreateEffect<MovieSearchState>(() => 
            _store
                .ObserveAction<PerformMovieSearchAction>()
                .Select(x => _omdbMovieService.SearchAsync(x.SearchText))
                .Switch()
                .Select(x => new PerformMovieSearchFulfilledAction(x)), 
            true);
    }
}
