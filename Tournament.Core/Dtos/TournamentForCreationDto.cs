// TournamentForCreationDto.cs
using System.ComponentModel.DataAnnotations;

public class TournamentForCreationDto
{
    [Required]
    public string Title { get; set; } = null!;

    [Required]
    public DateTime StartDate { get; set; }

    public List<GameForCreationInTournamentControllerDto> Games { get; set; } = new();
}
