using System.Collections.Immutable;
using JE.Core.Dto;

namespace JE.App.State
{
    public class MovieSearchState
    {
        /// <summary>
        /// Search text user has entered
        /// </summary>
        public string SearchText { get; set; }
        
        /// <summary>
        /// Bool value indicating that search is in progress
        /// </summary>
        public bool IsSearching { get; set; }

        /// <summary>
        /// List of found Movies
        /// </summary>
        public ImmutableArray<OmdbMovieSearchDto> Movies { get; set; } = ImmutableArray<OmdbMovieSearchDto>.Empty;
    }
}
