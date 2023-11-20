using GraphQL.Models;
using GraphQL.Models.Responses;

namespace GraphQL
{
    public class Query
    {
        const int PAGESIZE = 1;

        public Studios? Studios([Service] DBContext dbContext, int page)
        {
            if (page < 1) return null;
            return new Studios()
            {
                Infos = new Infos()
                {
                    Count = dbContext.Studios.Count(),
                    Pages = (int)Math.Ceiling((double)(dbContext.Studios.Count() / PAGESIZE)),
                    PreviousPage = page == 1 ? null : page - 1,
                    NextPage = dbContext.Studios.Count() <= page * PAGESIZE ? null : page + 1
                },
                Results = dbContext.Studios.Skip((page - 1) * PAGESIZE).Take(PAGESIZE).ToList()
            };
        }

        public Studio? Studio([Service] DBContext dbContext, int id)
        {
            return dbContext.Studios.FirstOrDefault(s => s.Id == id);
        }

        public Editors? Editors([Service] DBContext dbContext, int page)
        {
            if(page < 1) return null;
            return new Editors()
            {
                Infos = new Infos()
                {
                    Count = dbContext.Editors.Count(),
                    Pages = (int)Math.Ceiling((double)(dbContext.Editors.Count() / PAGESIZE)),
                    PreviousPage = page == 1 ? null : page - 1,
                    NextPage = dbContext.Editors.Count() <= page*PAGESIZE ? null : page + 1
                },
                Results = dbContext.Editors.Skip((page - 1) * PAGESIZE).Take(PAGESIZE).ToList()
            };
        }

        public Editor? Editor([Service] DBContext dbContext, int id)
        {
            return dbContext.Editors.FirstOrDefault(e => e.Id == id);
        }
    }
}
