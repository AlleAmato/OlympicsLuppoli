using Olympics.Controllers;
using Olympics.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Olympics.ViewModels
{
    internal class PartecipationsViewModel : BaseViewModel
    {
        #region Proprietà per tabella partecipazioni
        private List<Partecipation> partecipations;

		public List<Partecipation> PartecipationsList
		{
			get { return partecipations; }
			set { partecipations = value; NotifyPropertyChanged("PartecipationsList"); }
		}
        #endregion

		# region Proprietà per filtri
        private string _nameFilter;

		public string NameFilter
		{
			get { return _nameFilter; }
			set { _nameFilter = value; NotifyPropertyChanged("NameFilter"); LoadData(); }
        }

        private string _sexFilter;

        public string SexFilter
        {
            get { return _sexFilter; }
            set { _sexFilter = value; NotifyPropertyChanged("SexFilter"); LoadData(); }
        }

		private string _gamesFilter;

		public string GamesFilter
		{
			get { return _gamesFilter; }
			set 
			{ 
				_gamesFilter = value;
				SportsList = Partecipations.GetAllSports(value);
				NotifyPropertyChanged("GamesFilter"); LoadData(); 

			}
        }

		private string _sportFilter;

		public string SportFilter
		{
			get { return _sportFilter; }
			set { 
				_sportFilter = value;
				EventsList = Partecipations.GetAllEvents(GamesFilter,value);
				NotifyPropertyChanged("SportFilter"); LoadData();
            }
        }

		private string _eventFilter;		

		public string EventFilter
		{
			get { return _eventFilter; }
			set { _eventFilter = value; NotifyPropertyChanged("EventFilter"); LoadData(); }
        }

		private string _medalFilter;

		public string MedalFilter
		{
			get { return _medalFilter; }
			set { _medalFilter = value; NotifyPropertyChanged("MedalFilter"); LoadData(); }
        }

		private int _pageSize;

		public int PageSize
		{
			get { return _pageSize; }
			set { _pageSize = value; NotifyPropertyChanged("PageSize"); LoadData(); }
		}

		#endregion

		#region Proprietà per valori combo
		private List<string> _sexList;

		public List<string> SexList
		{
			get { return _sexList; }
			set { _sexList = value; NotifyPropertyChanged("SexList"); }
        }

		private List<string> _gamesList;

		public List<string> GamesList
		{
			get { return _gamesList; }
			set { _gamesList = value; NotifyPropertyChanged("GamesList"); }
        }

		private List<string> _sportsList;

		public List<string> SportsList
		{
			get { return _sportsList; }
			set { _sportsList = value; NotifyPropertyChanged("SportsList"); }
		}

		private List<string> _eventsList;

		public List<string> EventsList 
		{
			get { return _eventsList; }
			set { _eventsList = value; NotifyPropertyChanged("EventsList"); }
		}

		private List<string> _medalsList;

		public List<string> MedalsList
		{
			get { return _medalsList; }
			set { _medalsList = value; NotifyPropertyChanged("MedalsList"); }
        }

		private List<int> _pageSizesList;

		public List<int> PageSizesList
		{
			get { return _pageSizesList; }
			set { _pageSizesList = value; NotifyPropertyChanged("PageSizesList"); }
		}
		#endregion

		#region Gestione paginazione
		private int _page=1;

		public int Page
		{
			get { return _page; }
			set 
			{
				int old = _page;

				_page = value;
				if (value <= 1)
					_page = 1;
				if (value > Pages)
					_page = Pages;

				if (old != _page) LoadData(false);

				NotifyPropertyChanged("Page");
                NotifyPropertyChanged("PaginationLabel");
                NotifyPropertyChanged("isNotFirstPage");
                NotifyPropertyChanged("isNotLastPage");
            }
		}

		private int _pages;

		public int Pages
		{
			get { return _pages; }
			set { _pages = value; NotifyPropertyChanged("Pages"); NotifyPropertyChanged("PaginationLabel"); }
        }

		public string PaginationLabel
		{
			get { return $"Pagina {Page} di {Pages}"; }
		}

		public bool isNotFirstPage
		{
			get { return Page > 1; }
		}
        public bool isNotLastPage
        {
            get { return Page != Pages; }
        }

        #endregion
        public PartecipationsViewModel()
		{
			SexList = Partecipations.GetAllSex();
			GamesList = Partecipations.GetAllGames();
			MedalsList = Partecipations.GetAllMedals();

			PageSizesList = new List<int>();
			PageSizesList.Add(10);
            PageSizesList.Add(20);
            PageSizesList.Add(50);
			PageSize = 10;
		}

		public void LoadData(bool resetPage=true)
		{
			PaginatedList<Partecipation> pl = Partecipations.GetPartecipations(NameFilter, SexFilter, GamesFilter, SportFilter, EventFilter, MedalFilter, Page, PageSize);
			PartecipationsList = pl.Data;
			Pages = (int) Math.Ceiling( (double) pl.TotalCount / PageSize);
			if(resetPage) Page = 1;
        }

        internal void FirstPage()
		{
			Page = 1;
		}

		internal void PrevPage()
		{
			Page--;
		}

		internal void NextPage()
		{
			Page++;
		}

		internal void LastPage()
		{
			Page = Pages;
		}

		internal void AzzeraFiltri()
		{
			_nameFilter = null;
			_sexFilter = null;
			_gamesFilter = null;
			_sportFilter = null;
			_eventFilter = null;
			_medalFilter = null;
			SportsList = new List<string>();
			EventsList = new List<string>();
			NotifyPropertyChanged("NameFilter");
            NotifyPropertyChanged("SexFilter");
            NotifyPropertyChanged("GamesFilter"); 
            NotifyPropertyChanged("SportFilter");
            NotifyPropertyChanged("EventFilter");
            NotifyPropertyChanged("MedalFilter");
			LoadData();
        }

		internal void About()
		{
			MessageBox.Show("E80 2022 - Prova intermedia!");
		}
	}
}
