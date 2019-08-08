using Flurl;
using Flurl.Http;
using JE.Core.Dto;
using JE.Core.Options;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JE.Infrastructure.Services
{
    public class OmdbMovieService : IOmdbMovieService
    {
        private readonly string _apiKey;

        public OmdbMovieService(OmdbOptions omdbOptions)
        {
            _apiKey = omdbOptions.ApiKey;
        }

        public async Task<IEnumerable<OmdbMovieSearchDto>> SearchAsync(string searchText)
        {
            var response = await CreateBaseUrl()
                .SetQueryParam("s", searchText)
                .SetQueryParam("type", "movie")
                .GetJsonAsync<OmdbMovieSearchResponseDto>()
                .ConfigureAwait(false);

            return response.Search;
        }

        public Task<OmdbMovieDto> GetAsync(string id) =>
            CreateBaseUrl()
                .SetQueryParam("i", id)
                .GetJsonAsync<OmdbMovieDto>();

        private Url CreateBaseUrl() => 
            "http://www.omdbapi.com"
                .SetQueryParam("apikey", _apiKey)
                .SetQueryParam("r", "json");
    }
}
