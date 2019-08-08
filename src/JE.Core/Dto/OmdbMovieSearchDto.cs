using System;

namespace JE.Core.Dto
{
    public class OmdbMovieSearchDto
    {
        public string Title { get; set; }

        public long Year { get; set; }

        public string ImdbId { get; set; }

        public string Type { get; set; }

        public Uri Poster { get; set; }
    }
}
