using JE.Core.Dto;
using ReduxSimple;
using System.Collections.Generic;
using System.Collections.Immutable;
using static ReduxSimple.Reducers;

namespace JE.App.State
{
    public static class MovieSearchReducers
    {
        public static IEnumerable<On<MovieSearchState>> CreateReducers()
        {
            return new List<On<MovieSearchState>>
            {
                On<ResetMovieSearchAction, MovieSearchState>((state, action) => new MovieSearchState
                {
                    SearchText = string.Empty,
                    IsSearching = false,
                    Movies = ImmutableArray<OmdbMovieSearchDto>.Empty
                }),
                On<PerformMovieSearchAction, MovieSearchState>((state, action) => new MovieSearchState
                {
                    SearchText = action.SearchText,
                    IsSearching = true,
                    Movies = ImmutableArray<OmdbMovieSearchDto>.Empty
                }),
                On<PerformMovieSearchFulfilledAction, MovieSearchState>((state, action) => new MovieSearchState
                {
                    SearchText = state.SearchText,
                    IsSearching = false,
                    Movies = action.Movies.ToImmutableArray()
                })
            };
        }
    }
}
