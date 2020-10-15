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
        public Color Color { get; set; }

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
