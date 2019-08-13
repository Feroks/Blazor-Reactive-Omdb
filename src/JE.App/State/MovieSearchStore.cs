using JE.Core.Dto;
using ReduxSimple;
using System.Collections.Immutable;

namespace JE.App.State
{
    public class MovieSearchStore : ReduxStore<MovieSearchState>
	{
		protected override MovieSearchState Reduce(MovieSearchState state, object action)
		{
			switch (action)
			{
				case ResetMovieSearchAction _:
					return ReduceResetMovieSearch();
				case PerformMovieSearchAction rAction:
					return ReducePerformMovieSearch(rAction);
				case PerformMovieSearchFulfilledAction rAction:
					return ReducePerformMovieSearchFulfilled(state, rAction);
			}
			
			return base.Reduce(state, action);
		}

		private static MovieSearchState ReduceResetMovieSearch() =>
			new MovieSearchState
			{
				SearchText = string.Empty,
				IsSearching = false,
				Movies = ImmutableArray<OmdbMovieSearchDto>.Empty
			};

		private static MovieSearchState ReducePerformMovieSearch(PerformMovieSearchAction rAction) =>
            new MovieSearchState
            {
                SearchText = rAction.SearchText,
                IsSearching = true,
                Movies = ImmutableArray<OmdbMovieSearchDto>.Empty
            };

        private static MovieSearchState ReducePerformMovieSearchFulfilled(MovieSearchState state, PerformMovieSearchFulfilledAction rAction) =>
            new MovieSearchState
            {
                SearchText = state.SearchText,
                IsSearching = false,
                Movies = rAction.Movies.ToImmutableArray()
            };
    }
}