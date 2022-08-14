using System.Text.Json.Serialization;

namespace EFRelationships
{
    public class Character
    {
        public int id { get; set; }

        public string name { get; set; } = string.Empty;

        public string RpgClass { get; set; } = "Knight";

        //1:n with User
        [JsonIgnore]
        public User User { get; set; }

        public int UserId { get; set; }
        //1:1
        public Weapon Weapon { get; set; }
        //n:m
        public List<Skill> Skills { get; set; }

    }
}
