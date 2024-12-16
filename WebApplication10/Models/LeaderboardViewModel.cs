using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace LeaderboardFrontend.Models
{
    public class LeaderboardViewModel
    {
        public long Id { get; set; }
        public int Rank { get; set; }

        [Required]
        public string? Username { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Sayfa sayısı pozitif bir değer olmalıdır.")]
        public int TotalPages { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Sayfa sayısı pozitif bir değer olmalıdır.")]
        public int TodayPages { get; set; }
        public string? Streak { get; set; }
    }

}
