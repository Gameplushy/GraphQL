namespace GraphQL.Models.Responses
{
    [GraphQLDescription("Wrapper object for general Studio queries")]
    public class Studios
    {
        [GraphQLDescription("Misc information about the query")]
        public Infos? Infos {  get; set; }
        [GraphQLDescription("Result of the query")]
        public List<Studio>? Results { get; set; }
    }
}
