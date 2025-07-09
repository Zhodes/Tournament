public class TournamentForUpdateDto
{
    public string Title { get; set; } = null!;
    public DateTime StartDate { get; set; }

    public List<int> GameIds { get; set; } = new();
}
