namespace GraphQL.Models.Responses
{
    public class Infos
    {
        public int Count {  get; set; }
        public int Pages { get; set; }
        public int? NextPage { get; set; }
        public int? PreviousPage { get; set; }
    }
}
