namespace JE.Core.Dto
{
    public class OmdbMovieSearchResponseDto
    {
        public OmdbMovieSearchDto[] Search { get; set; }

        public long TotalResults { get; set; }

        public string Response { get; set; }
    }
}
