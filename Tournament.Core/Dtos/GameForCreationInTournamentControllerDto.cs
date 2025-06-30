// GameForCreationDto.cs
using System.ComponentModel.DataAnnotations;

public class GameForCreationInTournamentControllerDto
{
    [Required]
    public string Title { get; set; } = null!;

    [Required]
    public DateTime StartDate { get; set; }
}
