using System.ComponentModel.DataAnnotations;

public class GameForUpdateDto
{
    public int Id { get; set; }

    [Required]
    [MaxLength(60)]
    public string Title { get; set; } = null!;

    [Required]
    public DateTime Time { get; set; }

    [Required]
    public int TournamentDetailsId { get; set; }
}
