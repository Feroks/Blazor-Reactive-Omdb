using System;

namespace JE.Core.Dto
{
    public class OmdbMovieSearchDto
    {
        public string Title { get; set; }

        public string Year { get; set; }

        public string ImdbId { get; set; }

        public string Type { get; set; }

        public Uri Poster { get; set; }
    }
}
