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

          
            return GetAnyCategory(category, percent);
        }
        public static string TranscriptBasicSyntax(string source, int percent)
        {
            string final = string.Empty;

            foreach (string item in source.Split(SyntaxSeparator))
            {
                var trimmed = item.Trim();
               
                if (Enum.IsDefined(typeof(Category), trimmed) && (trimmed != "Age" || trimmed != "Phone" || trimmed != "Date"))
                {
                    final += $"'{GetBasicTranscription(trimmed, percent)}',";
                }
            }

            return final;
        }

        public static string Transcript(string source, string[] rules)
        {
            string final = string.Empty;



            return final;
        }


        #region Basic Transciptors
        private static string GetAnyCategory(string categoryName, int percent)
        {
            Random random = new Random();
            string final = string.Empty;


            var s = File.ReadAllLines(categoryName + Properties.Resources.Formatter);


            double t = (Convert.ToDouble((s.Length - 1)) / 100) * percent;

         

       
            return s[random.Next(0, Convert.ToInt32(t))];


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
