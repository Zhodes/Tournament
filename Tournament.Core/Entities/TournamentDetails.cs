using System.ComponentModel.DataAnnotations;

namespace Tournament.Core.Entities
{
    public class TournamentDetails
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is a required field.")]
        [MaxLength(60, ErrorMessage = "Maximum length for the Title is 30 characters.")]
        public string Title { get; set; } = null!;

        [Required(ErrorMessage = "StartDate is a required field.")]
        public DateTime StartDate { get; set; }
        public ICollection<Game> Games { get; set; } = null!;
    }
}
