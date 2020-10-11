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


            return GetBasicCategory(category, percent);
        }
        public static string Transcript(string source, int percent, string[] custom)
        {
            string final = string.Empty;

            foreach (string item in source.Split(SyntaxSeparator))
            {
                var trimmed = item.Trim();

                if (Enum.IsDefined(typeof(Category), trimmed))
                    final += $"{GetBasicTranscription(trimmed, percent)},";
                else 
                {
                    if (File.Exists(Properties.Resources.PathToCustomDir + "\\" + trimmed + Properties.Resources.Formatter))
                    {
                        if (File.ReadAllLines(Properties.Resources.PathToCustomDir + "\\" + trimmed + Properties.Resources.Formatter).Length == 0)
                        {
                            throw new Exception($"Fill your custom file: {trimmed}");
                        }
                        final += $"{GetCustomCategory(trimmed, percent)},";
                    }
                    
                }
            }

            return final;
        }


        private static string GetCustomCategory(string categoryName, int percent)
        {
            Random random = new Random();

            var path = Properties.Resources.PathToCustomDir + "\\" + categoryName + Properties.Resources.Formatter;

            var s = File.ReadAllLines(path);


            double t = (Convert.ToDouble((s.Length)) / 100) * percent;




            return $"'{s[random.Next(0, Convert.ToInt32(t))]}'";


        }

        #region General transcriptor Transciptors
        private static string GetBasicCategory(string categoryName, int percent)
        {
            Random random = new Random();
            string final = string.Empty;


            var s = File.ReadAllLines(categoryName + Properties.Resources.Formatter);


            double t = (Convert.ToDouble((s.Length)) / 100) * percent;

         

       
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
