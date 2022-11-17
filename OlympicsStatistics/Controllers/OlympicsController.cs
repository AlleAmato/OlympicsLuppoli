using OlympicsStatistics.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OlympicsStatistics.Controllers
{
    internal static class OlympicsController
    {
        public async static Task<List<string>> getAllNocs()
        {
            //Recuperare tutti i NOC ordinati in modo alfabetico
            using( OlympicsContext context = new OlympicsContext())
            {
                return await context.Partecipations
                    .Select(s => s.NOC)
                    .Distinct()
                    .OrderBy(ob => ob)
                    .ToListAsync();
            }
        }

        public async static Task<List<AthleteWithMedals>> getAthlets(string noc, bool onlyMedalists, int page, int pageSize)
        {
            if (page <= 0) page = 1;

            using( OlympicsContext context = new OlympicsContext() )
            {
                return await context.AthletesNFs
                    .Where(q => noc == null || q.Partecipations.Any(a => a.NOC == noc))
                    .Select(s => new AthleteWithMedals
                    {
                        IdAthlete = s.IdAthlete,
                        Name = s.Name,
                        Golds = s.Medals.Where(w => w.Medal1 == "Gold").Count(),
                        Silvers = s.Medals.Where(w => w.Medal1 == "Silver").Count(),
                        Bronzes = s.Medals.Where(w => w.Medal1 == "Bronze").Count()
                    })
                    .Where(w => w.Golds + w.Silvers + w.Bronzes > 0 || onlyMedalists == false)
                    .OrderByDescending(ob => ob.Golds)
                    .ThenByDescending(ob => ob.Silvers)
                    .ThenByDescending(ob => ob.Bronzes)
                    .ThenBy(ob => ob.Name)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();
            }
        }

        public async static Task AddNew()
        {
            using(var context = new OlympicsContext() )
            {
                try
                {
                    //Inserisco Gianmarco Tamberi che ha vinto l'oro nel salto in alto nel 2021
                    //AthletsNf --> Recuperare IdAthleta
                    AthletesNF a = await context.AthletesNFs
                        .FirstOrDefaultAsync(w => w.Name == "Gianmarco Tamberi");


                    //Events --> Recuperare la gara
                    Event e = await context.Events.FirstOrDefaultAsync(w => w.Id == 575);


                    //Games --> Nuova riga
                    Game g = new Game
                    {
                        Games = "2020 Summer",
                        Year = 2021,
                        Season = "Summer"
                    };
                    context.Games.Add(g);
                    await context.SaveChangesAsync();

                    //g = await context.Games
                    //    .FirstOrDefaultAsync(w => w.Year == 2021);

                    //Partecipations --> Nuova riga
                    Partecipation p = new Partecipation
                    {
                        //IdAthlete = a.IdAthlete,
                        Id = await context.Partecipations.MaxAsync(mx=>mx.Id) + 1,
                        AthletesNF = a,
                        Age = 29,
                        NOC = "ITA",
                        //IdEvent = e.Id,
                        Event = e,
                        //IdGame = g.Id,
                        Game = g,
                        City = "Tokyo"
                    };
                    context.Partecipations.Add(p);

                    //Medals --> Nuova riga
                    Medal m = new Medal
                    {
                        Medal1 = "Gold",
                        //IdAthlete = a.IdAthlete,
                        AthletesNF = a,
                        //IdEvent = e.Id,
                        Event = e,
                        //IdGame = g.Id
                        Game = g
                    };
                    context.Medals.Add(m);
                    await context.SaveChangesAsync();



                    //Inseriamo Tamberi anche nelle olimpiadi del 2024
                    Partecipation p2024 = new Partecipation
                    {
                        Id = await context.Partecipations.MaxAsync(mx => mx.Id) + 1,
                        AthletesNF = a,
                        Event = e,
                        Age = 33,
                        NOC = "ITA",
                        City = "Paris",
                        Game = new Game
                        {
                            Games = "2024 Summer",
                            Year = 2024,
                            Season = "Summer"
                        }
                    };
                    context.Partecipations.Add(p2024);
                    await context.SaveChangesAsync();
                }
                catch(Exception e)
                {
                    throw;
                }
            }
        }
    }
}
