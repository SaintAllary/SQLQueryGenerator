using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace AutoGeneratorSQL
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public DispatcherTimer Generator { get; set; }
        public bool DoesNeedToStart { get; set; }

        public string[] Names { get; set; }
        public string[] Senames { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            DataContext = new ViewModel();
            PostInitialize();

        }

        public void PostInitialize()
        {
            try
            {
                Generator = new DispatcherTimer();
                Generator.Tick += GenerateValues;

                CheckFile(Properties.Resources.PathToNames);

                
                CheckFile(Properties.Resources.PathToSenames);

                Names = File.ReadAllLines(Properties.Resources.PathToNames);
                Senames = File.ReadAllLines(Properties.Resources.PathToSenames);


            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }






        }

        private void CheckFile(string FilePath)
        {
            if (!File.Exists(FilePath))
            {
                throw new Exception($"You lost {FilePath} file, try to relog");
            }
        }

        private void GenerateValues(object sender, EventArgs e)
        {
            if (ClipboardState.IsChecked.Value)
            {
                GenerateClipboardValue();
            }
        }

        private void GenerateClipboardValue()
        {
            //MessageBox.Show(Enum.Parse(typeof(Category), CategoryBox.SelectedItem.ToString()).ToString());
            string s = string.Empty;
            int f, t;
            if (int.TryParse(FromBox.Text, out f) && int.TryParse(ToBox.Text, out t))
            {
                if (t > f)
                {
                    switch (Enum.Parse(typeof(Category), CategoryBox.SelectedItem.ToString()))
                    {
                        case Category.Name:
                            s = CreateRandomName();
                            break;
                        case Category.Sename:
                            s = CreateRandomSename();
                            break;
                        case Category.Age:
                            s = CreateRandomAge();
                            break;
                        case Category.Phone:
                            s = CreateRandomPhone();
                            break;
                        case Category.Operator:
                            s = CreateRandomOperator();
                            break;
                        case Category.Country:
                            s = CreateRandomCountry();
                            break;
                        case Category.City:
                            s = CreateRandomCity();
                            break;
                        case Category.Street:
                            s = CreateRandomStreet();
                            break;
                        case Category.Company:
                            s = CreateRandomCompany();
                            break;
                        case Category.Position:
                            s = CreateRandomPosition();
                            break;
                        case Category.Date:
                            s = CreateRandomDate();
                            break;
                        default:
                            break;
                    }
                    Clipboard.SetText(s);
                    ValueClipboardBox.Text = s;
                }
                else
                    SetState("Invalid range", Colors.Red);

            }

        }



        #region Creating
        private string CreateRandomPhone()
        {
            SuccesesGenerate();
            var r = new Random();
            var s = $"+{r.Next(0, 10)}{r.Next(0, 10)}({Enum.GetNames(typeof(OperatorsCode))[new Random().Next(0, Enum.GetNames(typeof(OperatorsCode)).Length - 1)].Substring(1, 3)}){r.Next(0, 10)}{r.Next(0, 10)}{r.Next(0, 10)}{r.Next(0, 10)}{r.Next(0, 10)}{r.Next(0, 10)}";
            return s;
        }
        private string CreateRandomOperator()
        {
            SuccesesGenerate();

            if (int.Parse(ToBox.Text) > Enum.GetValues(typeof(Operators)).Length - 1)
            {
                SetState($"Max is { Enum.GetValues(typeof(Operators)).Length - 1}", Colors.DarkOrange);
                return "NULL";
            }

            return Enum.GetNames(typeof(Operators))[new Random().Next(0, int.Parse(ToBox.Text))];
        }
        private string CreateRandomAge()
        {

            int f = 0;
            int t = 0;
            if (int.TryParse(FromBox.Text, out f) && int.TryParse(ToBox.Text, out t))
                if (f < t)
                {
                    SuccesesGenerate();
                    return new Random().Next(f, t).ToString();
                }

            SetState("Invalid range", Colors.Red);
            return string.Empty;


        }

        private string CreateRandomName()
        {
            int f, t = 0;
            if (int.TryParse(FromBox.Text, out f) && int.TryParse(ToBox.Text, out t))
            {
                if (!BasicCheckValues(Names))
                    return "NULL";
            }


            SuccesesGenerate();
            return Names[new Random().Next(f, t)];
        }

        private string CreateRandomSename()
        {
            int f, t = 0;
            if (int.TryParse(FromBox.Text, out f) && int.TryParse(ToBox.Text, out t))
            {
                if (!BasicCheckValues(Senames))
                    return "NULL";
            }


            SuccesesGenerate();
            return Senames[new Random().Next(f, t)];
        }

        private string CreateRandomCountry()
        {
            int f, t = 0;
            var s = (DataContext as ViewModel).Countries;
            if (int.TryParse(FromBox.Text, out f) && int.TryParse(ToBox.Text, out t))
            {
                if (!BasicCheckValues(s))
                {
                    return "NULL";
                }
            }


            SuccesesGenerate();
            return s[new Random().Next(f, t)];
        }

        private string CreateRandomCity()
        {
            int f, t = 0;
            var s = (DataContext as ViewModel).Cities;
            if (int.TryParse(FromBox.Text, out f) && int.TryParse(ToBox.Text, out t))
            {
                if (!BasicCheckValues(s))
                    return "NULL";
            }


            SuccesesGenerate();
            return s[new Random().Next(f, t)];
        }
        private string CreateRandomStreet()
        {

            int f, t = 0;
            var s = (DataContext as ViewModel).Streets;
            if (int.TryParse(FromBox.Text, out f) && int.TryParse(ToBox.Text, out t))
            {
                if (!BasicCheckValues(s))
                    return "NULL";
            }


            SuccesesGenerate();
            return s[new Random().Next(f, t)];
        }

        private string CreateRandomDate()
        {

            var year = new Random().Next(1975, 2004);
            var month = new Random().Next(1, 13);
            SuccesesGenerate();
          return  $"{year}.{month}.{new Random().Next(0, DateTime.DaysInMonth(year, month))}";
        }

        private string CreateRandomPosition()
        {
            int f, t = 0;
            var s = (DataContext as ViewModel).Positions;
            if (int.TryParse(FromBox.Text, out f) && int.TryParse(ToBox.Text, out t))
            {
                if (!BasicCheckValues(s.ToArray()))
                    return "NULL";
            }


            SuccesesGenerate();
            return s[new Random().Next(f, t)];
        }

        private string CreateRandomCompany()
        {
            int f, t = 0;
            var s = (DataContext as ViewModel).Companies;
            if (int.TryParse(FromBox.Text, out f) && int.TryParse(ToBox.Text, out t))
            {
                if (!BasicCheckValues(s))
                    return "NULL";
            }


            SuccesesGenerate();
            return s[new Random().Next(f, t)];
        }


        #endregion







        #region Clicks


        private void ChangeTimerState(string value, Color color)
        {
            TimerState.Foreground = new SolidColorBrush(color);
            TimerState.Text = value;
        }

        private void ClipboardState_Click(object sender, RoutedEventArgs e)
        {
            if (!ClipboardState.IsChecked.Value)
                SetState("Enabled", Colors.DarkGray);
            else
            {
                SetState("On waiting", Colors.DarkOrange);
            }
        }

        private void IntervalBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (DoesNeedToStart)
                SetTimer();

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SetTimer();
            DoesNeedToStart = true;
            ChangeTimerState("On", Colors.Green);
        }
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Generator.Stop();
            ChangeTimerState("Off", Colors.DarkGray);
            if (ClipboardState.IsChecked.Value)
                SetState("Stopped", Colors.DarkOrange);

        }
        #endregion

        #region Optimize
        private bool BasicCheckValues(string[] value)
        {
            int f, t = 0;
            if (int.TryParse(FromBox.Text, out f) && int.TryParse(ToBox.Text, out t))
            {
                if (int.Parse(ToBox.Text) > value.Length)
                {
                    SetState($"Max value is {value.Length}", Colors.Red);
                    return false;
                }
                else if (t < 0 || f < 0)
                {
                    SetState($"USE ONLY POSITIVE", Colors.Red);
                    return false;

                }
                return true;
            }

            return false;

        }
        private void SetState(string value, Color color)
        {
            ClipboardStateBlock.Foreground = new SolidColorBrush(color);
            ClipboardStateBlock.Text = value;
        }
        private void SetTimer()
        {
            uint t = 0;
            if (uint.TryParse(IntervalBox.Text, out t))
            {
                if (t > 0)
                {
                    Generator.Stop();
                    Generator.Interval = TimeSpan.FromMilliseconds(t);
                    Generator.Start();
                }
                else
                    IntervalBox.Text = "1";


            }
        }
        private void SuccesesGenerate()
        {
            SetState("Generating..", Colors.Green);
        }

        private void SetDepends(string value, Color color)
        {
            DependingBox.Foreground = new SolidColorBrush(color);
            DependingBox.Text = value;
        }
        private void ChangeDependingState()
        {
            switch (Enum.Parse(typeof(Category), CategoryBox.SelectedItem.ToString()))
            {
                case Category.Name:
                    SetDepends("Yes", Colors.DarkOrange);
                    break;
                case Category.Sename:
                    SetDepends("Yes", Colors.DarkOrange);
                    break;
                case Category.Age:
                    SetDepends("Yes", Colors.DarkOrange);
                    break;
                case Category.Phone:
                    SetDepends("No", Colors.DarkGray);
                    break;
                case Category.Operator:
                    SetDepends("Yes", Colors.DarkOrange);
                    break;
                case Category.Country:
                    SetDepends("Yes", Colors.DarkOrange);
                    break;
                case Category.City:
                    SetDepends("Yes", Colors.DarkOrange);
                    break;
                case Category.Street:
                    SetDepends("Yes", Colors.DarkOrange);
                    break;
                case Category.Company:
                    SetDepends("Yes", Colors.DarkOrange);
                    break;
                case Category.Position:
                    SetDepends("Yes", Colors.DarkOrange);
                    break;
                case Category.Date:
                    SetDepends("No", Colors.DarkGray);
                    break;
                default:
                    break;
            }
        }


        #endregion

        private void CategoryBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            ChangeDependingState();
        }
    }
}






