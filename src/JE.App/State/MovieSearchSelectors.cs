using JE.Core.Dto;
using ReduxSimple;
using System.Collections.Immutable;
using static ReduxSimple.Selectors;

namespace JE.App.State
{
    public static class MovieSearchSelectors
    {
        public static ISelectorWithoutProps<MovieSearchState, bool> SelectIsSearching = CreateSelector<MovieSearchState, bool>(state => state.IsSearching);

        public static ISelectorWithoutProps<MovieSearchState, ImmutableArray<OmdbMovieSearchDto>> SelectMovies = CreateSelector<MovieSearchState, ImmutableArray<OmdbMovieSearchDto>>(state => state.Movies);

        public static ISelectorWithoutProps<MovieSearchState, string> SelectSearchText = CreateSelector<MovieSearchState, string>(state => state.SearchText);
    }
}
