using System.ComponentModel.DataAnnotations;

namespace GraphQL.Models
{
    [GraphQLDescription("Video game studio")]
    public class Studio
    {
        [Key]
        [GraphQLType(typeof(IdType))]
        [GraphQLDescription("Unique Identifier")]
        public int Id { get; set; }
        [GraphQLDescription("Name of the game studio")]
        public required string Name { get; set; }
        [GraphQLDescription("List of games it has published")]
        public List<Game>? Games { get; set; }
    }
}
