using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AutoGeneratorSQL
{
    static class SyntaxTranscriptor
    {
        static char SyntaxSeparator;
        static SyntaxTranscriptor()
        {
            SyntaxSeparator = ',';
        }

        public  static string GetBasicTranscription(string category, int percent)
        {
            #region special cases
            if (category == Category.Age.ToString())
            return new Random().Next(0, percent).ToString();
            else if (category == Category.Date.ToString())
            {
                var year = new Random().Next(1975, 2004);
                var month = new Random().Next(1, 13);
                return $"{year}.{month}.{new Random().Next(0, DateTime.DaysInMonth(year, month))}";
            }
            else if (category == Category.Phone.ToString())
            {
                var s = File.ReadAllLines(category+Properties.Resources.Formatter);
                var r = new Random();
                double t = (Convert.ToDouble((s.Length - 1)) / 100) * percent;

                var s1 = $"+{r.Next(0, 10)}{r.Next(0, 10)}({s[r.Next(0, Convert.ToInt32(t))]})" +
                    $"{r.Next(0, 10)}{r.Next(0, 10)}{r.Next(0, 10)}{r.Next(0, 10)}{r.Next(0, 10)}{r.Next(0, 10)}";
                return s1;
            }
            #endregion


            return GetAnyCategory(category, percent);
        }
        public static string TranscriptBasicSyntax(string source, int percent)
        {
            string final = string.Empty;

            foreach (string item in source.Split(SyntaxSeparator))
            {
                var trimmed = item.Trim();
               
                if (Enum.IsDefined(typeof(Category), trimmed))
                    final += $"{GetBasicTranscription(trimmed, percent)},";
            }

            return final;
        }

        public static string Transcript(string source,int percent, string[] rules)
        {
            string final = string.Empty;



            return final;
        }


        #region General transcriptor Transciptors
        private static string GetAnyCategory(string categoryName, int percent)
        {
            Random random = new Random();
            string final = string.Empty;


            var s = File.ReadAllLines(categoryName + Properties.Resources.Formatter);


            double t = (Convert.ToDouble((s.Length - 1)) / 100) * percent;

         

       
            return $"'{s[random.Next(0, Convert.ToInt32(t))]}'";


        }
        #endregion
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
}
