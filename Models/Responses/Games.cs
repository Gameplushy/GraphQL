namespace GraphQL.Models.Responses
{
    [GraphQLDescription("Wrapper object for general Game queries")]
    public class Games
    {
        [GraphQLDescription("Misc information about the query")]
        public Infos? Infos {  get; set; }
        [GraphQLDescription("Result of the query")]
        public List<Game>? Results { get; set; }
    }
}
