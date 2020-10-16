using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace AutoGeneratorSQL
{
    class Syntax
    {
        //public Color Color { get; set; }


        private Color color;

        public Color Color
        {
            get {
                Brush = new SolidColorBrush(color);
                return color; }
            set {
             
                color = value;
                Brush = new SolidColorBrush(color);
            }
        }


        private Brush brush;

        public Brush Brush
        {
            get { return brush; }
            set { brush = value; }
        }


        private string word;

        public string Word
        {
            get { 
                if (!File.Exists(Properties.Resources.PathToCustomDir + "\\" + word + Properties.Resources.Formatter))
                    Color = Colors.DarkGray;
                else
                    Color = Colors.DarkOrange;
                return word;
            }
            set {
                word = value;
                if (!File.Exists(Properties.Resources.PathToCustomDir + "\\"+ word+Properties.Resources.Formatter))
                    Color = Colors.DarkGray;
                else
                    Color = Colors.DarkOrange;

            }
        }

    }
}
