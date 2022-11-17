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
    }
}
