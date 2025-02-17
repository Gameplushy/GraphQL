using System.ComponentModel.DataAnnotations;

namespace GraphQL.Models
{
    [GraphQLDescription("Entity for a video game")]
    public class Game
    {
        [Key]
        [GraphQLType(typeof(IdType))]
        [GraphQLDescription("Unique Identifier")]
        public int Id { get; set; }
        [GraphQLDescription("Name of the game")]
        public required string Name { get; set; }
        [GraphQLDescription("List of the game's genre (FPS, RPG ...)")]
        public required List<string> Genres { get; set; }
        [GraphQLDescription("Date of publcation in YYYYMMDD format")]
        public int? PublicationDate { get; set; }
        [GraphQLDescription("List of game editors that has contributes to this game")]
        public required List<Editor> Editors { get; set; }
        [GraphQLDescription("List of game studios that has published this game")]
        public required List<Studio> Studios { get; set; }
        [GraphQLDescription("List of platforms this game is compatible to")]
        public required List<string> Platforms { get; set; }
    }
}
