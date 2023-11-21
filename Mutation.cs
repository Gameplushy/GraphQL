using GraphQL.Models;
using GraphQL.Models.Responses;
using Microsoft.EntityFrameworkCore;

namespace GraphQL
{
    public class Mutation
    {
        public Editor? CreateEditor([Service] DBContext dBContext, string name, List<int>? gameIds)
        {
            if (gameIds != null)
            {
                gameIds = gameIds.Distinct().ToList();
                if (!gameIds.All(gid => dBContext.Games.Any(g => g.Id == gid)))
                    return null;
            }

            var newEditor = dBContext.Editors.Add(new Editor() { Name = name, Games = gameIds?.Select(id => dBContext.Games.Single(g => g.Id == id)).ToList() });
            dBContext.SaveChanges();
            return newEditor.Entity;
        }

        public Editor? EditEditor([Service] DBContext dBContext, [ID] int id, string? name, List<int>? gameIds)
        {
            Editor? editor = dBContext.Editors.Include(e => e.Games).SingleOrDefault(e => e.Id == id);
            if (editor == null) return null;
            if (name != null)
                editor.Name = name;
            if (gameIds != null)
            {
                gameIds = gameIds.Distinct().ToList();
                if (!gameIds.All(gid => dBContext.Games.Any(g => g.Id == gid)))
                    return null;
                //Weird thing that I have to do because I can't directly give a new List of int or there is an issue with primary keys...
                List<Game> currentGameBase = editor.Games!;
                int gameCounter = 0;
                while (gameCounter < currentGameBase.Count)
                {
                    if (!gameIds.Contains(currentGameBase[gameCounter].Id))
                    {
                        currentGameBase.Remove(currentGameBase[gameCounter]);
                    }
                    else
                    {
                        gameIds.Remove(currentGameBase[gameCounter].Id);
                        gameCounter++;
                    }
                }
                currentGameBase.AddRange(gameIds!.Select(id => dBContext.Games.Single(g => g.Id == id)));
            }
            dBContext.SaveChanges();
            return editor;
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

        public Studio? EditStudio([Service] DBContext dBContext, [ID] int id, string? name, List<int>? gameIds)
        {
            Studio? studio = dBContext.Studios.Include(s => s.Games).SingleOrDefault(s => s.Id == id);
            if (studio == null) return null;
            if (name != null)
                studio.Name = name;
            if (gameIds != null)
            {
                gameIds = gameIds.Distinct().ToList();
                if (!gameIds.All(gid => dBContext.Games.Any(g => g.Id == gid)))
                    return null;
                //Weird thing that I have to do because I can't directly give a new List of int or there is an issue with primary keys...
                List<Game> currentGameBase = studio.Games!;
                int gameCounter = 0;
                while (gameCounter < currentGameBase.Count)
                {
                    if (!gameIds.Contains(currentGameBase[gameCounter].Id))
                    {
                        currentGameBase.Remove(currentGameBase[gameCounter]);
                    }
                    else
                    {
                        gameIds.Remove(currentGameBase[gameCounter].Id);
                        gameCounter++;
                    }
                }
                currentGameBase.AddRange(gameIds!.Select(id => dBContext.Games.Single(g => g.Id == id)));
            }
            dBContext.SaveChanges();
            return studio;
        }

        public Game? CreateGame([Service] DBContext dBContext, string name, List<string> genres, int? publicationDate, List<int> editors, List<int> studios, List<string> platforms)
        {
            genres = genres.Distinct().ToList();
            editors = editors.Distinct().ToList();
            studios = studios.Distinct().ToList();
            platforms = platforms.Distinct().ToList();
            if (!(studios.All(s => dBContext.Studios.Any(stu => stu.Id == s))) || !(editors.All(e => dBContext.Editors.Any(edi => edi.Id == e))))
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

        public Game? EditGame([Service] DBContext dBContext, [ID] int id, string? name, List<string>? genres, int? publicationDate, List<int>? editors, List<int>? studios, List<string>? platforms)
        {
            Game? game = dBContext.Games.Include(g=>g.Editors).Include(g=>g.Studios).SingleOrDefault(g => g.Id == id);
            if (game == null) return null;

            genres = genres?.Distinct().ToList();
            platforms = platforms?.Distinct().ToList();

            if (name != null)
                game.Name = name;
            if(publicationDate != null)
                game.PublicationDate = publicationDate;
            if(genres != null)
                game.Genres = genres;
            if (platforms != null)
                game.Platforms = platforms;

            if(editors != null)
            {
                editors = editors.Distinct().ToList();
                if (!(editors.All(e => dBContext.Editors.Any(edi => edi.Id == e))))
                    return null;
                List<Editor> currentEditorBase = game.Editors!;
                int editorCounter = 0;
                while (editorCounter < currentEditorBase.Count)
                {
                    if (!editors.Contains(currentEditorBase[editorCounter].Id))
                    {
                        currentEditorBase.Remove(currentEditorBase[editorCounter]);
                    }
                    else
                    {
                        editors.Remove(currentEditorBase[editorCounter].Id);
                        editorCounter++;
                    }
                }
                currentEditorBase.AddRange(editors!.Select(id => dBContext.Editors.Single(g => g.Id == id)));
            }

            if (studios != null)
            {
                studios = studios.Distinct().ToList();
                if (!(studios.All(e => dBContext.Studios.Any(edi => edi.Id == e))))
                    return null;
                List<Studio> currentStudioBase = game.Studios!;
                int studioCounter = 0;
                while (studioCounter < currentStudioBase.Count)
                {
                    if (!studios.Contains(currentStudioBase[studioCounter].Id))
                    {
                        currentStudioBase.Remove(currentStudioBase[studioCounter]);
                    }
                    else
                    {
                        studios.Remove(currentStudioBase[studioCounter].Id);
                        studioCounter++;
                    }
                }
                currentStudioBase.AddRange(studios!.Select(id => dBContext.Studios.Single(g => g.Id == id)));
            }
            dBContext.SaveChanges();
            return game;
        }
    }
}
