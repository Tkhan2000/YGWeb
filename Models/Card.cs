using System.ComponentModel.DataAnnotations;

namespace YGWeb.Models
{
    public class Card
    {
        public enum cardType
        {
            Monster,
            Spell,
            Trap
        }
        public enum cardRace
        {
            Aqua, 
            Beast, 
            BeastWarrior, 
            Cyberse, 
            Dinosaur, 
            DivineBeast, 
            Dragon, 
            Fairy, 
            Fiend, 
            Fish, 
            Insect, 
            Machine, 
            Plant, 
            Psychic, 
            Pyro, 
            Reptile, 
            Rock, 
            SeaSerpent, 
            Spellcaster, 
            Thunder, 
            Warrior, 
            WingedBeast, 
            Wyrm, 
            Zombie
        }

        public enum cardAttribute
        {
            Dark, 
            Divine, 
            Earth, 
            Fire, 
            Light, 
            Water, 
            Wind
        }
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
