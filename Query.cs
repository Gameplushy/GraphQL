using GraphQL.Models;
using GraphQL.Models.Responses;
using Microsoft.EntityFrameworkCore;

namespace GraphQL
{
    public class Query
    {
        const int PAGESIZE = 15;

        [GraphQLDescription("Fetch list of games, using certain filters if needed")]
        public Games? Games([Service] DBContext dBContext, 
            [GraphQLDescription("(Optional) Page number. Size of page is 15")] int? page, 
            [GraphQLDescription("(Optional) Filter by game genre")] string? genre, 
            [GraphQLDescription("(Optional) Filter by platform")] string? platform, 
            [GraphQLDescription("(Optional) Filter by game studio")] string? studio)
        {
            page = page ?? 1;
            if (page < 1) return null;
            IEnumerable<Game> targetedGames = dBContext.Games.Include(g=>g.Editors).Include(g=>g.Studios);
            if (genre != null) targetedGames = targetedGames.Where(g => g.Genres.Contains(genre));
            if (platform != null) targetedGames = targetedGames.Where(g => g.Platforms.Contains(platform));
            if (studio != null) targetedGames = targetedGames.Where(g => g.Studios.Any(s=> s.Name == studio));
            return new Games()
            {
                Infos = new Infos()
                {
                    Count = targetedGames.Count(),
                    Pages = (int)Math.Ceiling((double)(targetedGames.Count() / PAGESIZE)),
                    PreviousPage = page == 1 ? null : page - 1,
                    NextPage = targetedGames.Count() <= page * PAGESIZE ? null : page + 1
                },
                Results = targetedGames.Skip(((int)page - 1) * PAGESIZE).Take(PAGESIZE).ToList()
            };
        }

        [GraphQLDescription("Fetch all studios, by pages of 15")]
        public Studios? Studios([Service] DBContext dbContext, 
           [GraphQLDescription("(Optional) A positive page number")] int page = 1)
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
                Results = dbContext.Studios.Include(s=>s.Games).Skip((page - 1) * PAGESIZE).Take(PAGESIZE).ToList()
            };
        }

        [GraphQLDescription("Fetch a specific studio by Id")]
        public Studio? Studio([Service] DBContext dbContext, 
            [GraphQLDescription("The studio's Id")] int id)
        {
            return dbContext.Studios.Include(s=>s.Games).FirstOrDefault(s => s.Id == id);
        }

        [GraphQLDescription("Fetch all editors, by pages of 15")]
        public Editors? Editors([Service] DBContext dbContext,
            [GraphQLDescription("(Optional) A positive page number")] int page = 1)
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
                Results = dbContext.Editors.Include(e=>e.Games).Skip((page - 1) * PAGESIZE).Take(PAGESIZE).ToList()
            };
        }

        [GraphQLDescription("Fetch a specific editor by Id")]
        public Editor? Editor([Service] DBContext dbContext,
            [GraphQLDescription("The editor's Id")] int id)
        {
            return dbContext.Editors.Include(e=>e.Games).FirstOrDefault(e => e.Id == id);
        }
    }
}
