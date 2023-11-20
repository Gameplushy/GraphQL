using GraphQL.Models;

namespace GraphQL
{
    public class Query
    {
        public Studio? Studio([Service] DBContext dbContext, int id)
        {
            return dbContext.Studios.FirstOrDefault(s => s.Id == id);
        }

        public Editor? Editor([Service] DBContext dbContext, int id)
        {
            return dbContext.Editors.FirstOrDefault(e => e.Id == id);
        }
    }
}
