using System.Collections.Immutable;
using JE.Core.Dto;

namespace JE.App.State
{
    public class MovieSearchState
    {
        public string SearchText { get; set; }
        
        public bool IsSearching { get; set; }

        public ImmutableArray<OmdbMovieSearchDto> Movies { get; set; } = ImmutableArray<OmdbMovieSearchDto>.Empty;
    }
}
