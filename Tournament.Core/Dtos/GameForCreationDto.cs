using System.ComponentModel.DataAnnotations;

public class GameForCreationDto
{
    [Required]
    public string Title { get; set; } = null!;

    [Required]
    public DateTime Time { get; set; }

    [Required]
    public int TournamentDetailsId { get; set; }
}