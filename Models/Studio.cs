using System.ComponentModel.DataAnnotations;

namespace GraphQL.Models
{
    public class Studio
    {
        [Key]
        [GraphQLType(typeof(IdType))]
        public int Id { get; set; }
        public required string Name { get; set; }
        public List<Game>? Games { get; set; }
    }
}
