using System.ComponentModel.DataAnnotations;

namespace GraphQL.Models
{
    [GraphQLDescription("Video game editor")]
    public class Editor
    {
        [Key]
        [GraphQLType(typeof(IdType))]
        [GraphQLDescription("Unique Identifier")]
        public int Id { get; set; }
        [GraphQLDescription("Name of the editor")]
        public required string Name { get; set; }
        [GraphQLDescription("List of games it has contributed")]
        public List<Game>? Games { get; set; }
    }
}
