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

        public ObservableCollection<Syntax> Syntaxes { get; set; }

 



        #region Propfull
    


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
                Category = new ObservableCollection<string>();

                Syntaxes = new ObservableCollection<Syntax>();

                foreach (var item in File.ReadAllLines(Properties.Resources.CustomSyntaxesPath))
                {
                    Syntaxes.Add(new Syntax() { Word=  item });
                }
             


                foreach (var item in Enum.GetNames(typeof(Category)).ToList())
                    Category.Add(item);

                RangeTo = 10;
                TimerInterval = 1000;
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





}
