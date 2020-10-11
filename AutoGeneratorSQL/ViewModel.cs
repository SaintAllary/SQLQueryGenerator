using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AutoGeneratorSQL
{
    class ViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<string> Category { get; set; }

        private string[] countries;

        private string[] cities;

        private string[] streets;

        private string[] companies;

        public string[] Companies
        {
            get { return companies; }
            set { companies = value; OnPropertyChanged("Companies"); }
        }

        private string wayToSave;

     





        public ObservableCollection<string> Positions { get; set; }


        #region Propfull
        public string WayToSave
        {
            get { return wayToSave; }
            set { wayToSave = value; OnPropertyChanged("WayToSave"); }
        }
        public string[] Streets
        {
            get { return streets; }
            set { streets = value; OnPropertyChanged("Cities"); }
        }


        public string[] Cities
        {
            get { return cities; }
            set { cities = value; OnPropertyChanged("Cities"); }
        }


        public string[] Countries
        {
            get { return countries; }
            set { countries = value; OnPropertyChanged("Countries"); }
        }



        private uint timerInterval;

        public uint TimerInterval
        {
            get { return timerInterval; }
            set { timerInterval = value; OnPropertyChanged("TimerInterval"); }
        }


        private int rangeFrom;
        public int RangeFrom
        {
            get { return rangeFrom; }
            set { rangeFrom = value; OnPropertyChanged("RangeFrom"); }
        }
        private int rangeTo;

        public int RangeTo
        {
            get { return rangeTo; }
            set { rangeTo = value; OnPropertyChanged("RangeTo"); }
        }


        #endregion


        public ViewModel()
        {
            try
            {
                Countries = File.ReadAllLines(Properties.Resources.PathToCountries);
                Cities = File.ReadAllLines(Properties.Resources.PathToCities);
                Category = new ObservableCollection<string>();
                Streets = File.ReadAllLines(Properties.Resources.PathToStreets);
                Positions = new ObservableCollection<string>();
                Companies = File.ReadAllLines(Properties.Resources.PathToCompanies);

                foreach (var item in Enum.GetNames(typeof(Positions)))
                {
                    Positions.Add(item);
                }

                foreach (var item in Enum.GetNames(typeof(Category)).ToList())
                {
                    Category.Add(item);
                }
                RangeTo = 10;
                TimerInterval = 500;
            }
            catch (Exception EX)
            {

                MessageBox.Show(EX.Message);
            }

        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

    }

    enum Positions
    {
        Admin,
        User,
        Director,
        Dispetcher


    }

    enum Category
    {
        Name,
        Sename,
        Age,
        Phone,
        Operator,
        Country,
        City,
        Street,
        Company,
        Position,
        Date

    }
    enum Operators
    {
        Vodafone,
        Kievstar,
        Life,
        MTS,
        Beline,
        Megafone,
        Tele2,
        Yota,
        Jio,
        Airtel,
        Vi,
        BSNL,
        Telus,
        Bell,
        Rogers,
        Fido


    }

    enum OperatorsCode
    {
        V050,
        V066,
        V095,
        V099,
        K039,
        K067,
        K068,
        K096,
        K097,
        K098,
        L063,
        L073,
        L092,
        L091,
        L094,
        L070




    }


}
