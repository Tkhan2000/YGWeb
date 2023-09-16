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

        public int atk { get; set; }

        public int def { get; set; }
        [Range(1,12)]
        public int level { get; set; }

        public string race { get; set; }

        public string attribute { get; set; }

        public string archetype { get; set; }
        public string desc { get; set; }

        public string image_url { get; set; }

    }
}
