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

        public ObservableCollection<Syntax> Syntaxes { get; set; }

 



        #region Propfull
    


        private uint timerInterval;

        public uint TimerInterval
        {
            get { return timerInterval; }
            set { timerInterval = value; OnPropertyChanged("TimerInterval"); }
        }
        #endregion


        public ViewModel()
        {
            try
            {


                Syntaxes = new ObservableCollection<Syntax>();

                foreach (var item in File.ReadAllLines(Properties.Resources.CustomSyntaxesPath))
                    Syntaxes.Add(new Syntax() { Word=  item });
             




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





}
