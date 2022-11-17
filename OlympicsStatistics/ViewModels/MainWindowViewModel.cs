using OlympicsStatistics.Controllers;
using OlympicsStatistics.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace OlympicsStatistics.ViewModels
{
    internal class MainWindowViewModel : BaseViewModel
    {
        #region Binding properties
        private List<string> _nocs;

        public List<string> Nocs
        {
            get { return _nocs; }
            set { _nocs = value; NotifyPropertyChanged("Nocs"); }
        }

        private string _selectedNoc;

        public string SelectedNoc
        {
            get { return _selectedNoc; }
            set { _selectedNoc = value; NotifyPropertyChanged("SelectedNoc"); LoadData(); }
        }

        private List<AthleteWithMedals> _athletes;

        public List<AthleteWithMedals> Athletes
        {
            get { return _athletes; }
            set { _athletes = value; NotifyPropertyChanged("Athletes"); }
        }

        private bool _onlyMedalists;

        public bool OnlyMedalists
        {
            get { return _onlyMedalists; }
            set { _onlyMedalists = value; NotifyPropertyChanged("OnlyMedalists"); LoadData(); }
        }

        private bool _isDataLoaded;

        public bool IsDataLoaded
        {
            get { return _isDataLoaded; }
            set { _isDataLoaded = value; NotifyPropertyChanged("IsDataLoaded"); }
        }
        #endregion

        #region Event Handling
        public void NextPage()
        {
            page++;
            LoadData();
        }

        public void PreviousPage()
        {
            page--;
            if (page <= 0) page = 1;
            LoadData();
        }
        #endregion

        #region private fields
        private int page = 1;
        private int pageSize = 50;
        #endregion

        public MainWindowViewModel()
        {
            InitializeData();
        }

        private async Task InitializeData()
        {
            string noc = ConfigurationManager.AppSettings["defaultNOC"];
            var t = OlympicsController.getAllNocs();
            LoadData(noc);


            Nocs = await t;
            _selectedNoc = noc;
            NotifyPropertyChanged("SelectedNoc");
        }

        private async Task LoadData(string noc)
        {
            IsDataLoaded = false;
            Athletes = await OlympicsController.getAthlets(noc, OnlyMedalists, page, pageSize);
            IsDataLoaded = true;
        }

        private async Task LoadData()
        {
            await LoadData(SelectedNoc);
        }
    }
}
