using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AutoGeneratorSQL
{
    static class SyntaxTranscriptor
    {
        private static RNGCryptoServiceProvider _RNG = new RNGCryptoServiceProvider();
        static char SyntaxSeparator;
        static SyntaxTranscriptor()
        {
            SyntaxSeparator = ',';
        }


        private static int GetNextRnd(int min, int max)
        {
            byte[] rndBytes = new byte[4];
            _RNG.GetBytes(rndBytes);
            int rand = BitConverter.ToInt32(rndBytes, 0);
            const Decimal OldRange = (Decimal)int.MaxValue - (Decimal)int.MinValue;
            Decimal NewRange = max - min;
            Decimal NewValue = ((Decimal)rand - (Decimal)int.MinValue) / OldRange * NewRange + (Decimal)min;
            return (int)NewValue;
        }
        public static string GetBasicTranscription(string category, int percent)
        {
            #region special cases
            if (category == BasicSyntax.Age.ToString())
                return GetNextRnd(0, percent).ToString();
            else if (category == BasicSyntax.Date.ToString())
            {
                var year = GetNextRnd(1975, 2004);
                var month = GetNextRnd(1, 13);
                return $"'{year}.{month}.{GetNextRnd(0, DateTime.DaysInMonth(year, month))}'";
            }
            else if (category == BasicSyntax.Phone.ToString())
            {
                var s = File.ReadAllLines(category + Properties.Resources.Formatter);
                double t = (Convert.ToDouble((s.Length - 1)) / 100) * percent;

                var s1 = $"+{GetNextRnd(0, 10)}{GetNextRnd(0, 10)}({s[GetNextRnd(0, Convert.ToInt32(t))]})" +
                    $"{GetNextRnd(0, 10)}{GetNextRnd(0, 10)}{GetNextRnd(0, 10)}{GetNextRnd(0, 10)}{GetNextRnd(0, 10)}{GetNextRnd(0, 10)}";
                return s1;
            }
            else if (category == BasicSyntax.Money.ToString())
                return (Convert.ToDouble(GetNextRnd(0,int.MaxValue/10000) / 100) * (percent>0? percent:1)).ToString();
            #endregion


            return GetBasicCategory(category, percent);
        }
        public static string Transcript(string source, int percent)
        {
            string final = string.Empty;

            foreach (string item in source.Split(SyntaxSeparator))
            {
                var trimmed = item.Trim();

                if (Enum.IsDefined(typeof(BasicSyntax), trimmed))
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
        public static bool DoesAnySyntaxExist(string CustomWord) => Enum.IsDefined(typeof(BasicSyntax), CustomWord);
        private static string GetCustomCategory(string categoryName, int percent)
        {
            Random random = new Random();

            var path = Properties.Resources.PathToCustomDir + "\\" + categoryName + Properties.Resources.Formatter;

            var s = File.ReadAllLines(path);


            double t = (Convert.ToDouble((s.Length)) / 100) * percent;




            return $"'{s[GetNextRnd(0, Convert.ToInt32(t))]}'";


        }

        #region General transcriptor
        private static string GetBasicCategory(string categoryName, int percent)
        {
            var s = File.ReadAllLines(Properties.Resources.PathToDirBasic+"\\" +categoryName + Properties.Resources.Formatter);

            double t = (Convert.ToDouble((s.Length)) / 100) * percent;

            return $"'{s[GetNextRnd(0, Convert.ToInt32(t))]}'";


        }
        public static string GetFullFirstPart(string sourceString, string currentTable, List<string>customWords)
        {
            string tmp = $"INSERT INTO [{currentTable}] (";
                   
            foreach (var item in sourceString.Split(SyntaxSeparator))
            {
                var s = item.Trim();
   
                if (Enum.IsDefined(typeof(BasicSyntax), s) || customWords.Contains(s))
                {
                    tmp += $"[{s}],";

                }
            }
           tmp = tmp.Substring(0, tmp.Length - 1);
            tmp += ") VALUES "; ;
            return tmp;
        }
        public static string GetFullSecondPart(string firstPart, string values) => firstPart + $"({values});";

        #endregion
    }
    enum BasicSyntax
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
        Date,
        Money,
        Password
    }
}
