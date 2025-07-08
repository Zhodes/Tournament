using System.ComponentModel.DataAnnotations;

namespace Domain.Core.Entities
{
    public class Game
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is a required field.")]
        [MaxLength(60, ErrorMessage = "Maximum length for the Title is 30 characters.")]
        public string Title { get; set; } = null!;

        [Required(ErrorMessage = "StartDate is a required field.")]
        public DateTime Time { get; set; }
        public int TournamentDetailsId { get; set; }
        public TournamentDetails TournamentDetails { get; set; } = null!;
    }
}
