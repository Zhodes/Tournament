using Tournament.DTOs.Games;

namespace Tournament.DTOs.Tournaments
{
    public class TournamentDto
    {
        public string Title { get; set; } = null!;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<GameDto> Games { get; set; }

    }
}
