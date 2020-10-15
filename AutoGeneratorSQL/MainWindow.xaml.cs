using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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


        private void GenerateValues(object sender, EventArgs e)
        {

                QueryModule();
                SetQueryState(Properties.Resources.StateGenerating, Colors.Green);

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



            LogBox.Text = "";


        }
        private void FileSaver()
        {
            LogBox.SelectionStart = OutputHistory.Text.Length;
            LogBox.ScrollToEnd();
        }
        private void QueryGenerateValues()
        {
            try
            {
                var s = new TextRange(QueryRTB.Document.ContentStart, QueryRTB.Document.ContentEnd).Text;
                List<string> words = new List<string>();

                foreach (var item in (DataContext as ViewModel).Syntaxes)
                {
                    words.Add(item.Word);
                }
            
                OutPutBox.Text = SyntaxTranscriptor.Transcript(s, Convert.ToInt32((SliderPercent.Value)), words.ToArray());

                if (OutPutBox.Text.Length - 1 > 0)
                {
                    OutPutBox.Text = OutPutBox.Text.Remove(OutPutBox.Text.Length - 1, 1);

                    HistoryOutput = OutPutBox.Text + "\n";
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

     
      
       
        private void RichTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

            var s = (sender as RichTextBox);

            // basic 
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

            //custom
            foreach (var item in s.Document.Blocks)
            {
                bool f = false;
                foreach (var inneritem in (DataContext as ViewModel).Syntaxes)
                {

                    TextManipulation.FromTextPointer(item.ContentStart, item.ContentEnd, inneritem.Word, item.FontStyle, FontWeights.Bold, new SolidColorBrush(inneritem.color), item.Background, item.FontSize);
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

        #region Clicks


        private void ChangeTimerState(string value, Color color)
        {
            TimerState.Foreground = new SolidColorBrush(color);
            TimerState.Text = value;
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
            ChangeTimerState(Properties.Resources.StateOn, Colors.Green);

                SetQueryState(Properties.Resources.StateoOnWaiting, Colors.DarkOrange);
 
        }

        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as ToggleButton).IsChecked.Value)
            {
                SetQueryState(Properties.Resources.StateoOnWaiting, Colors.DarkOrange);

            }
            else
            {
                SetQueryState(Properties.Resources.StateEnable, Colors.DarkGray);
            }

        }
        #endregion

        #region Optimize

        private void SetQueryState(string value, Color color)
        {
            QueryState.Foreground = new SolidColorBrush(color);
            QueryState.Text = value;
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

        private void GenerateCustom(object sender, RoutedEventArgs e)
        {
            var s = File.ReadAllLines(Properties.Resources.CustomSyntaxesPath);
            if (!s.Contains(CustomTextBox.Text.Trim()) && CustomTextBox.Text.Trim().Length >= 3 && CustomTextBox.Text.All(x => char.IsLetter(x)))
            {
                File.AppendAllLines(Properties.Resources.CustomSyntaxesPath, new string[] { CustomTextBox.Text.Trim() });
                (DataContext as ViewModel).Syntaxes.Add(new Syntax() {Word = CustomTextBox.Text.Trim() });
                CustomTextBox.Text = "";
            }
        }

        private void StopTimer(object sender, RoutedEventArgs e)
        {
            ChangeTimerState(Properties.Resources.TimerStopped, Colors.DarkOrange);
            SetQueryState(Properties.Resources.TimerStopped, Colors.DarkOrange);
            Generator.Stop();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
    }


}





