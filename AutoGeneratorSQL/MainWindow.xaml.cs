using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
        public string[] Senames { get; set; }
        public bool DoesNeedToStart { get; set; }

        public string[] Names { get; set; }
        private string historyOutput;

        public string HistoryOutput
        {
            get { return historyOutput; }
            set
            {
                historyOutput += value;
                OutputHistory.Text = historyOutput;
                OutputHistory.SelectionStart = OutputHistory.Text.Length;
                OutputHistory.ScrollToEnd();
            }
        }


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
                SavePath.Text = "Logs\\Log.txt";

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }






        }
        private void FileSaver()
        {
            SetQuerySavePathState("File exist", Brushes.Green);
            File.AppendAllLines(SavePath.Text, new string[]{ OutPutBox.Text});
            LogBox.Text += "----Wrote in file----\n";
            LogBox.SelectionStart = OutputHistory.Text.Length;
            LogBox.ScrollToEnd();
        }
        private void QueryGenerateValues()
        {
            try
            {
                var s = new TextRange(QueryRTB.Document.ContentStart, QueryRTB.Document.ContentEnd).Text;

                OutPutBox.Text = SyntaxTranscriptor.TranscriptBasicSyntax(s, Convert.ToInt32((SliderPercent.Value)));

                if (OutPutBox.Text.Length - 1 > 0)
                {
                    OutPutBox.Text = OutPutBox.Text.Remove(OutPutBox.Text.Length - 1, 1);

                    HistoryOutput = OutPutBox.Text + "\n";

                    if (HistoryCheck.IsChecked.Value)
                    FileSaver();


                }
            }
            catch (Exception ex)
            {

                LogBox.Text += ex.Message + "\n";
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
            if (QueryMode.IsChecked.Value)
            {
                QueryModule();
                SetQueryState("Generating..", Colors.Green);
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


        private void CategoryBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            ChangeDependingState();
        }

        private void RichTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

            var s = (sender as RichTextBox);


            foreach (var item in s.Document.Blocks)
            {
                bool f = false;
                foreach (var inneritem in Enum.GetNames(typeof(Category)))
                {

                    TextManipulation.FromTextPointer(item.ContentStart, item.ContentEnd, inneritem, item.FontStyle, FontWeights.Bold, Brushes.Blue, item.Background, item.FontSize);
                    f = true;
                }
                if (f)
                    break;

            }

            TextManipulation.FromTextPointer(s.Document.ContentStart, s.Document.ContentEnd, " ", new FontStyle(), FontWeights.Normal, Brushes.Black, null, 12);
        }

        private void QueryModule()
        {
            QueryGenerateValues();
        }

        #region Creating
        private string CreateRandomPhone()
        {
            var s = DataContext as ViewModel;
            SuccesesGenerate();
            var r = new Random();
            var s1 = $"+{r.Next(0, 10)}{r.Next(0, 10)}({s.Phone[new Random().Next(0, s.Phone.Length - 1)]})" +
                $"{r.Next(0, 10)}{r.Next(0, 10)}{r.Next(0, 10)}{r.Next(0, 10)}{r.Next(0, 10)}{r.Next(0, 10)}";
            return s1;
        }
        private string CreateRandomOperator()
        {
            SuccesesGenerate();
            var s = DataContext as ViewModel;

            if (int.Parse(ToBox.Text) > s.Operators.Length - 1)
            {
                SetState($"Max is { s.Operators.Length - 1}", Colors.DarkOrange);
                return "NULL";
            }

            return s.Operators[new Random().Next(0, int.Parse(ToBox.Text))];
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
            return $"{year}.{month}.{new Random().Next(0, DateTime.DaysInMonth(year, month))}";
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
            {
                SetState("Enabled", Colors.DarkGray);
                SetQueryState("Enabled", Colors.DarkGray);

            }

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
            if (QueryMode.IsChecked.Value)
            {
                SetQueryState("On waiting..", Colors.DarkOrange);
            }
        }
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Generator.Stop();
            ChangeTimerState("Off", Colors.DarkGray);
            if (ClipboardState.IsChecked.Value)
                SetState("Stopped", Colors.DarkOrange);
            if (QueryMode.IsChecked.Value)
            {
                SetQueryState("Stopped", Colors.DarkOrange);
            }

        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {

            try
            {
                File.Exists(SavePath.Text);
                File.Delete(SavePath.Text);
                File.Create(SavePath.Text);
            }
            catch (Exception ex)
            {

                LogBox.Text += ex.Message + "\n";
            }
      

            #region Test

            //foreach (var item in QueryRTB.Document.Blocks)
            //{
            //    foreach (var inneritem in Enum.GetNames(typeof(Category)))
            //    {

            //        TextManipulation.FromTextPointer(item.ContentStart, item.ContentEnd, inneritem, item.FontStyle, FontWeights.Bold, Brushes.Blue, item.Background, item.FontSize);
            //    }
            //    item.Foreground = Brushes.Black;
            //}
            //QueryRTB.Foreground = Brushes.Black;



            //TextRange rangeOfText1 = new TextRange(QueryRTB.Document.ContentEnd, QueryRTB.Document.ContentEnd);
            //rangeOfText1.Text = "Text1 ";
            //rangeOfText1.ApplyPropertyValue(TextElement.ForegroundProperty, Brushes.Blue);
            //rangeOfText1.ApplyPropertyValue(TextElement.FontWeightProperty, FontWeights.Bold);

            //TextRange rangeOfWord = new TextRange(QueryRTB.Document.ContentEnd, QueryRTB.Document.ContentEnd);
            //rangeOfWord.Text = "word ";
            //rangeOfWord.ApplyPropertyValue(TextElement.ForegroundProperty, Brushes.Red);
            //rangeOfWord.ApplyPropertyValue(TextElement.FontWeightProperty, FontWeights.Regular);

            //TextRange rangeOfText2 = new TextRange(QueryRTB.Document.ContentEnd, QueryRTB.Document.ContentEnd);
            //rangeOfText2.Text = "Text2 ";
            //rangeOfText2.ApplyPropertyValue(TextElement.ForegroundProperty, Brushes.Blue);
            //rangeOfText2.ApplyPropertyValue(TextElement.FontWeightProperty, FontWeights.Bold);


            //QueryRTB.SelectAll();

            //QueryRTB.Selection.Text = "";


            //MessageBox.Show(RTBstring);

            //var RTBstring = new TextRange(QueryRTB.Document.ContentStart, QueryRTB.Document.ContentEnd).Text;
            //QueryRTB.Document.Blocks.Clear();
            //foreach (var item in RTBstring.Split(','))
            //{
            //    foreach (var inneritem in Enum.GetNames(typeof(Category)))
            //    {
            //        if (item == inneritem)
            //        {
            //            TextRange rangeOfText1 = new TextRange(QueryRTB.Document.ContentEnd, QueryRTB.Document.ContentEnd);
            //            rangeOfText1.Text = item;
            //            rangeOfText1.ApplyPropertyValue(TextElement.ForegroundProperty, Brushes.Blue);
            //            rangeOfText1.ApplyPropertyValue(TextElement.FontWeightProperty, FontWeights.Bold);

            //        }
            //        else
            //        {
            //            var RTBstring1 = new TextRange(QueryRTB.Document.ContentStart, QueryRTB.Document.ContentEnd).Text;

            //            if (RTBstring1.EndsWith(item))
            //            {
            //                TextRange rangeOfText1 = new TextRange(QueryRTB.Document.ContentEnd, QueryRTB.Document.ContentEnd);
            //                rangeOfText1.Text = item;
            //                rangeOfText1.ApplyPropertyValue(TextElement.ForegroundProperty, Brushes.Black);
            //                rangeOfText1.ApplyPropertyValue(TextElement.FontWeightProperty, FontWeights.Normal);
            //            }



            //        }



            //    }
            //    if (!(new TextRange(QueryRTB.Document.ContentStart, QueryRTB.Document.ContentEnd).Text.EndsWith(",")))
            //    {
            //        TextRange rangeOfText2 = new TextRange(QueryRTB.Document.ContentEnd, QueryRTB.Document.ContentEnd);
            //        rangeOfText2.Text = ",";
            //        rangeOfText2.ApplyPropertyValue(TextElement.ForegroundProperty, Brushes.Black);
            //        rangeOfText2.ApplyPropertyValue(TextElement.FontWeightProperty, FontWeights.Normal);
            //    }


            //}


            #endregion

        }

        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as CheckBox).IsChecked.Value)
            {
                SetQueryState("On waiting..", Colors.DarkOrange);
                CheckFile();

            }
            else
            {
                SetQuerySavePathState("Enabled", Brushes.DarkGray);
                SetQueryState("Enabled", Colors.DarkGray);
            }

        }
        #endregion

        #region Optimize


        private void SetQuerySavePathState(string str, Brush brushes)
        {
      
                QuerySavePathState.Text = str;
                QuerySavePathState.Foreground = brushes;
        
        
        }

        private void SetState(object sender, TextChangedEventArgs e)
        {
            CheckFile();
        }

        private void CheckFile()
        {
            if (!SavePath.Text.EndsWith(Properties.Resources.Formatter) || SavePath.Text.Length > 15)
                SetQuerySavePathState("Incorrect format of file", Brushes.Red);
            else if (!File.Exists(SavePath.Text))
                SetQuerySavePathState("File doesn't exist", Brushes.DarkOrange);
            else
                SetQuerySavePathState("File exist", Brushes.Green);
        }
        private void SetQueryState(string value, Color color)
        {
            QueryState.Foreground = new SolidColorBrush(color);
            QueryState.Text = value;
        }
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

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            LogBox.Text = "";
        }

        private void WriteToLOG(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            LogBox.Text += $"Use {Convert.ToInt32(e.NewValue)}% of data\n";
            LogBox.SelectionStart = OutputHistory.Text.Length;
            LogBox.ScrollToEnd();
        }
    }


}





