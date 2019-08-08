using JE.Core.Options;

namespace JE.Infrastructure.Services
{
    public class OmdbMovieService : IOmdbMovieService
    {
        private readonly string _apiKey;

        public OmdbMovieService(OmdbOptions omdbOptions)
        {
            _apiKey = omdbOptions.ApiKey;
        }
    }
}
