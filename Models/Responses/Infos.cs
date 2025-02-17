namespace GraphQL.Models.Responses
{
    [GraphQLDescription("Misc information about the query's result")]
    public class Infos
    {
        [GraphQLDescription("Total number of concerned entities. This is not limited by the page size")]
        public int Count {  get; set; }
        [GraphQLDescription("Number of pages the concerned entites are separated into")]
        public int Pages { get; set; }
        [GraphQLDescription("If it exists, number of the next page")]
        public int? NextPage { get; set; }
        [GraphQLDescription("If it exists, number of the previous page")]
        public int? PreviousPage { get; set; }
    }
}
