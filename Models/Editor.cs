using System.ComponentModel.DataAnnotations;

namespace GraphQL.Models
{
    public class Editor
    {
        [Key]
        [GraphQLType(typeof(IdType))]
        public int Id { get; set; }
        public required string Name { get; set; }
        public List<Game>? Games { get; set; }
    }
}
