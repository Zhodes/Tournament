namespace Tournament.Core.Dtos
{
    public class TournamentDto
    {
        public string Title { get; set; } = null!;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<GameDto> Games { get; set; }



    }
}
