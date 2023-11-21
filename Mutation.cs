using GraphQL.Models;

namespace GraphQL
{
    public class Mutation
    {
        public Editor? CreateEditor([Service] DBContext dBContext, string name, List<int>? gameIds)
        {
            if (gameIds != null)
                if (!dBContext.Games.All(g => gameIds.Contains(g.Id)))
                    return null;
            var newEditor = dBContext.Editors.Add(new Editor() { Name = name, Games = gameIds?.Select(id=>dBContext.Games.Single(g=>g.Id == id)).ToList() });
            dBContext.SaveChanges();
            return newEditor.Entity;
        }

        public Studio? CreateStudio([Service] DBContext dBContext, string name, List<int>? gameIds)
        {
            if (gameIds != null)
                if (!dBContext.Games.All(g => gameIds.Contains(g.Id)))
                    return null;
            var newStudio = dBContext.Studios.Add(new Studio() { Name = name, Games = gameIds?.Select(id => dBContext.Games.Single(g => g.Id == id)).ToList() });
            dBContext.SaveChanges();
            return newStudio.Entity;
        }

    }
}
