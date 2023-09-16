using System.ComponentModel.DataAnnotations;

namespace YGWeb.Models
{
    public class Card
    {
        [Required]
        public int id { get; set; }
        [Required]
        public string name { get; set; }
        [Required]
        public string type { get; set; }

        public Nullable<int> atk { get; set; }

        public Nullable<int> def { get; set; }
        [Range(1,12)]
        public Nullable<int> level { get; set; }

        public string race { get; set; }

        public string? attribute { get; set; }

        public string? archetype { get; set; }
        public string description { get; set; }

        public string image_url { get; set; }

    }
}
