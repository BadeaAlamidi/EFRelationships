using System.Text.Json.Serialization;

namespace EFRelationships
{
    public class Skill
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Damage { get; set; }

        [JsonIgnore]
        public List <Character> Character { get; set; }

    }
}
