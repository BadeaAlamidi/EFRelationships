namespace EFRelationships
{
    public class User
    {
        public int Id { get; set; }
        public string UserName { get; set; }

        //1:n with character
        public List<Character> Characters { get; set; }

    }
}
