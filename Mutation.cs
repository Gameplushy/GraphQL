using GraphQL.Models;
using GraphQL.Models.Responses;
using Microsoft.EntityFrameworkCore;

namespace GraphQL
{
    public class Mutation
    {
        [GraphQLDescription("Creates an editor")]
        public Editor? CreateEditor([Service] DBContext dBContext, 
            [GraphQLDescription("Name of the editor")] string name, 
            [GraphQLDescription("(Optional) List of ids of the games this editor has contributed to")] List<int>? gameIds)
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

        [GraphQLDescription("Edits an already existing editor")]
        public Editor? EditEditor([Service] DBContext dBContext, 
            [GraphQLDescription("Id of the editor to edit")] [ID] int id, 
            [GraphQLDescription("(Optional) New name of the editor")] string? name,
            [GraphQLDescription("(Optional) New list of ids of games the editor has contributed to")]  List<int>? gameIds)
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

        [GraphQLDescription("Deletes an editor")]
        public bool DeleteEditor([Service] DBContext dbContext, [GraphQLDescription("Id of the editor")][ID] int id)
        {
            Editor? editor = dbContext.Editors.SingleOrDefault(e=>e.Id == id);
            if (editor == null) return false;
            dbContext.Remove(editor);
            List<Game> deleteGames = dbContext.Games.Include(g=>g.Editors).Where(g => g.Editors.Count == 0).ToList();
            dbContext.RemoveRange(deleteGames);
            dbContext.SaveChanges();
            return true;
        }

        [GraphQLDescription("Creates a studio")]
        public Studio? CreateStudio([Service] DBContext dBContext,
            [GraphQLDescription("Name of the studio")] string name,
            [GraphQLDescription("(Optional) List of ids of the games this studio has contributed to")] List<int>? gameIds)
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

        [GraphQLDescription("Edits an already existing studio")]
        public Studio? EditStudio([Service] DBContext dBContext, 
            [GraphQLDescription("Id of the editor to edit")] [ID] int id, 
            [GraphQLDescription("(Optional) New name of the studio")] string? name, 
            [GraphQLDescription("(Optional) New list of ids this studio has contributed to")]  List<int>? gameIds)
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

        [GraphQLDescription("Deletes a studio")]
        public bool DeleteStudio([Service] DBContext dbContext, [GraphQLDescription("Id of the studio")][ID] int id)
        {
            Studio? studio = dbContext.Studios.SingleOrDefault(s => s.Id == id);
            if (studio == null) return false;
            dbContext.Remove(studio);
            List<Game> deleteGames = dbContext.Games.Include(g => g.Studios).Where(g => g.Studios.Count == 0).ToList();
            dbContext.RemoveRange(deleteGames);
            dbContext.SaveChanges();
            return true;
        }

        [GraphQLDescription("Creates a game")]
        public Game? CreateGame([Service] DBContext dBContext, 
            [GraphQLDescription("Name of the game")] string name, 
            [GraphQLDescription("List of the game's genres")] List<string> genres, 
            [GraphQLDescription("(Optional) Publication date of the game in YYYYMMDD format")] int? publicationDate, 
            [GraphQLDescription("List of ids of editors who contributed to the game")] List<int> editors, 
            [GraphQLDescription("List of ids of studios who contributed to the game")] List<int> studios, 
            [GraphQLDescription("List of platforms you can play the game on")] List<string> platforms)
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

        [GraphQLDescription("Edits an already existing game")]
        public Game? EditGame([Service] DBContext dBContext, 
            [GraphQLDescription("Id of the game to edit")] [ID] int id, 
            [GraphQLDescription("(Optional) New name of the game")] string? name, 
            [GraphQLDescription("(Optional) New list of the game's genres")] List<string>? genres, 
            [GraphQLDescription("(Optional) New publication date of the game in YYYYMMDD format")] int? publicationDate, 
            [GraphQLDescription("(Optional) New list of ids of editors who contributed to the game")] List<int>? editors, 
            [GraphQLDescription("(Optional) New list of ids of studios who contributed to the game")] List<int>? studios, 
            [GraphQLDescription("(Optional) New list of platforms you can play the game on")] List<string>? platforms)
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

        [GraphQLDescription("Delete a game")]
        public bool DeleteGame([Service] DBContext dbContext, [GraphQLDescription("Id of the game")][ID] int id)
        {
            Game? game = dbContext.Games.SingleOrDefault(s => s.Id == id);
            if (game == null) return false;
            dbContext.Remove(game);
            dbContext.SaveChanges();
            return true;
        }
    }
}
