using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoGeneratorSQL
{
    static class SyntaxTranscriptor
    {
        static char SyntaxSeparator;
        static SyntaxTranscriptor()
        {
            SyntaxSeparator = ',';
        }

        static string TranscriptBasicSyntax(string source)
        {
            string final = string.Empty;

            foreach (var item in source.Split())
            {

            }

            return final;
        }

        static string Transcript(string source, string[] rules)
        {
            string final = string.Empty;



            return final;
        }

    }
}
