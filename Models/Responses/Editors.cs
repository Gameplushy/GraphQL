namespace GraphQL.Models.Responses
{
    [GraphQLDescription("Wrapper object for general Editor queries")]
    public class Editors
    {
        [GraphQLDescription("Misc information about the query")]
        public Infos? Infos {  get; set; }
        [GraphQLDescription("Result of the query")]

        public List<Editor>? Results { get; set; }
    }
}
