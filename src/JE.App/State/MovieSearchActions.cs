using JE.Core.Dto;
using JetBrains.Annotations;
using System.Collections.Generic;
using System.Linq;

namespace JE.App.State
{
    public class PerformMovieSearchAction
	{
		public PerformMovieSearchAction(string searchText)
		{
			SearchText = searchText;
		}

		public string SearchText { get; }
	}

	public class PerformMovieSearchFulfilledAction
	{
		public PerformMovieSearchFulfilledAction([CanBeNull] IEnumerable<OmdbMovieSearchDto> movies)
		{
			Movies = movies ?? Enumerable.Empty<OmdbMovieSearchDto>();
		}
		
        [NotNull]
		public IEnumerable<OmdbMovieSearchDto> Movies { get; }
	}
}