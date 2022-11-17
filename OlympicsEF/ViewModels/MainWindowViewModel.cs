using OlympicsEF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;
using System.Data.Entity;
using System.Runtime.Remoting.Contexts;
using System.Windows.Data;
using System.Globalization;

namespace OlympicsEF.ViewModels
{
	internal class MainWindowViewModel
    {
		private List<Medal> _dati;

		public List<Medal> Dati
		{
			get { return _dati; }
			set { _dati = value; }
		}

		public MainWindowViewModel()
		{
			using (OlympicsContext context = new OlympicsContext())
			{
				/*List<athlete> l = context.athletes
					//.Where(q => q.IdAthlete == 7)
					.Where(q => q.Year == 1992)
					.OrderBy(ob => ob.IdAthlete).ThenByDescending(ob => ob.Id)
					.Skip(10)
					.Take(1)
					.ToList();

				athlete primo = context.athletes
					//.Where(q=>q.Year==1992)
					.OrderBy(ob => ob.Name).FirstOrDefault(q => q.Year == 19920);


				//Età massima dei medagliati
				double? avg = context.athletes
					.Where(q => q.Medal != null)
					.Where(q => q.Age != null)
					.Average(q => q.Age);

				//Elenco delle nazioni partecipanti nel 2012
				var nazioni = context.athletes
					.Where(q=>q.Year==2012)
					.Select(s => new Nazione
					{
						NOC = s.NOC
						//s.NOC
					}).Distinct().OrderBy( ob=> ob.NOC ).ToList();
                Dati = nazioni;



                var p = context.athletes.Where(q => q.Year == 2012)
					.GroupBy(gb => new
					{
						gb.NOC
					})
					.Select(s => new
					{
						s.Key.NOC, Partecipations = s.Count()
					})
					.Where(q=> q.Partecipations > 100)
					.OrderBy(ob => ob.NOC)
					.ToList();
				*/

				//Tutti i medagliati
				/*var q1 = context.AthletesNFs
					.Where(q => q.Medals.Count()>0)
					.ToList();


				//Tutti i medagliati d'oro
				var q2 = context.AthletesNFs.Include(i => i.Medals)
					.Where( q=> q.Medals.Any( a => a.Medal1=="Gold" ) )
					.ToList();*/


				/*List<Medal> q3 = context.Medals
					.Include(i=>i.AthletesNF)
					.ToList();


				var q4 = context.Medals
					.Select(s => new
					{
						Medal = s.Medal1,
						Name = s.AthletesNF.Name
					}).ToList();

				Dati = q3;*/

				//Selezione i 10 atleti più giovani ad aver vinto una medaglia d'oro
				var q1 = context.athletes
					.Where(q => q.Medal == "Gold")
					.Where(q => q.Age != null)
					.Select(s => new
					{
						s.IdAthlete,
						s.Name,
						s.Age
					})
                    .Distinct()
                    .OrderBy(ob => ob.Age)
                    .Take(10).ToList();

				//Recuperare l'elenco delle città che hanno ospitato i giochi, in ordine alfabetico
				var q2 = context.athletes
					.Select(s => s.City )
					.Distinct()
					.OrderBy(s => s).ToList();


				//Quanti atleti hanno vinto la medaglia d'oro?
				var q3 = context.athletes
					.Where(q => q.Medal == "Gold")
					.Select(s => s.IdAthlete)
					.Distinct()
					.Count();


				//Per ogni città calcolare quante edizioni ha ospitato. Ordinare in ordine decrescente.
				var q4 = context.athletes
					.Select(s => new
					{
						s.City,
						s.Games
					})
					.GroupBy(gb => new
					{
						gb.City
					})
					.Select(s => new
					{
						s.Key.City,
						Editions = s.Distinct().Count()
					})
					.OrderByDescending(ob => ob.Editions)
					.ThenBy(ob => ob.City)
					.ToList();


				//Stilare la classifica degli sport che hanno assegnato più medaglie
				var q5 = context.Medals
					.Select(s => new
					{
						s.Event.Sport
					})
					.GroupBy(gb => new
					{
						gb.Sport
					})
					.Select(s => new
					{
						s.Key.Sport,
						Medals = s.Count()
					})
					.OrderByDescending(ob => ob.Medals)
					.ToList();


                //Per ogni edizione calcolare il numero totale di medaglie assegnate
                var q6 = context.Medals
                    .Select(s => s.Game.Games )
                    .GroupBy(gb => gb)
                    .Select(s => new
                    {
                        Games = s.Key,
                        Medals = s.Count()
                    })
                    .OrderByDescending(ob => ob.Medals)
                    .ToList();






            }
        }
	}
}
