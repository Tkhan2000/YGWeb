using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;

namespace YGWeb.Models
{
    // Create a list of cards representing a deck. Each deck can be represented as a list of card id's (or ints).
    public class Deck
    { 
        
        [Key]
        public int Id { get; set; }
        [Required]
        public string CardList { get; set; }
        [Required]
        public string YGWebUserId { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
