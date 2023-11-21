using GraphQL.Models;
using GraphQL.Models.Responses;

namespace GraphQL
{
    public class Mutation
    {
        public Editor? CreateEditor([Service] DBContext dBContext, string name, List<int>? gameIds)
        {
            if (gameIds != null)
            {
                gameIds = gameIds.Distinct().ToList();
                if (!gameIds.All(gid=> dBContext.Games.Any(g => g.Id == gid)))
                    return null;
            }

            var newEditor = dBContext.Editors.Add(new Editor() { Name = name, Games = gameIds?.Select(id=>dBContext.Games.Single(g=>g.Id == id)).ToList() });
            dBContext.SaveChanges();
            return newEditor.Entity;
        }

        public Studio? CreateStudio([Service] DBContext dBContext, string name, List<int>? gameIds)
        {
            if (gameIds != null)
            {
                gameIds = gameIds.Distinct().ToList();
                if (!gameIds.All(gid => dBContext.Games.Any(g => g.Id == gid)))
                    return null;
            }

            var newStudio = dBContext.Studios.Add(new Studio() { Name = name, Games = gameIds?.Select(id => dBContext.Games.Single(g => g.Id == id)).ToList() });
            dBContext.SaveChanges();
            return newStudio.Entity;
        }

        public Game? CreateGame([Service] DBContext dBContext, string name, List<string> genres, int? publicationDate, List<int> editors, List<int> studios, List<string> platforms)
        {
            genres = genres.Distinct().ToList();
            editors = editors.Distinct().ToList();
            studios = studios.Distinct().ToList();
            platforms = platforms.Distinct().ToList();
            if (!(studios.All(s=>dBContext.Studios.Any(stu=>stu.Id==s))) || !(editors.All(e=>dBContext.Editors.Any(edi => edi.Id == e))))
                return null;
            if (publicationDate != null)
            {
                if (!DateTime.TryParseExact(publicationDate.ToString(), "yyyyMMdd", null, System.Globalization.DateTimeStyles.None, out _))
                    return null;
            }
            var newGame = dBContext.Games.Add(new Game()
            {
                Name = name,
                PublicationDate = publicationDate,
                Genres = genres,
                Platforms = platforms,
                Editors = editors.Select(id => dBContext.Editors.Single(e => e.Id == id)).ToList(),
                Studios = studios.Select(id => dBContext.Studios.Single(s => s.Id == id)).ToList()
            });
            dBContext.SaveChanges();
            return newGame.Entity;
        }
    }
}
