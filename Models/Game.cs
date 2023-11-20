using System.ComponentModel.DataAnnotations;

namespace GraphQL.Models
{
    public class Game
    {
        [Key]
        [GraphQLType(typeof(IdType))]
        public int Id { get; set; }
        public required string Name { get; set; }
        public required List<string> Genres { get; set; }
        public int? PublicationDate { get; set; }
        public required List<Editor> Editors { get; set; }
        public required List<Studio> Studios { get; set; }
        public required List<string> Platforms { get; set; }
    }
}
