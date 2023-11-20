using GraphQL.Models;
using GraphQL.Models.Responses;
using Microsoft.EntityFrameworkCore;

namespace GraphQL
{
    public class Query
    {
        const int PAGESIZE = 15;

        public Games? Games([Service] DBContext dBContext, int? page, string? genre, string? platform, string? studio)
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

        public Studios? Studios([Service] DBContext dbContext, int? page)
        {
            page = page ?? 1;
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
                Results = dbContext.Studios.Include(s=>s.Games).Skip(((int)page - 1) * PAGESIZE).Take(PAGESIZE).ToList()
            };
        }

        public Studio? Studio([Service] DBContext dbContext, int id)
        {
            return dbContext.Studios.Include(s=>s.Games).FirstOrDefault(s => s.Id == id);
        }

        public Editors? Editors([Service] DBContext dbContext, int? page)
        {
            page = page ?? 1;
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
                Results = dbContext.Editors.Include(e=>e.Games).Skip(((int)page - 1) * PAGESIZE).Take(PAGESIZE).ToList()
            };
        }

        public Editor? Editor([Service] DBContext dbContext, int id)
        {
            return dbContext.Editors.Include(e=>e.Games).FirstOrDefault(e => e.Id == id);
        }
    }
}
