using JE.Core.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JE.Infrastructure.Services
{
    public interface IOmdbMovieService
    {
        Task<IEnumerable<OmdbMovieSearchDto>> SearchAsync(string searchText);

        Task<OmdbMovieDto> GetAsync(string id);
    }
}
