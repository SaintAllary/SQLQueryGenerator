﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace AutoGeneratorSQL
{
    class TextManipulation
    {
        /// <summary>
        /// Is manipulating a specific string inside of a TextPointer Range (TextBlock, TextBox...)
        /// </summary>
        /// <param name="startPointer">Starting point where to look</param>
        /// <param name="endPointer">Endpoint where to look</param>
        /// <param name="keyword">This is the string you want to manipulate</param>
        /// <param name="fontStyle">The new FontStyle</param>
        /// <param name="fontWeight">The new FontWeight</param>
        /// <param name="foreground">The new foreground</param>
        /// <param name="background">The new background</param>
        /// <param name="fontSize">The new FontSize</param>
        public static void FromTextPointer(TextPointer startPointer, TextPointer endPointer, string keyword, FontStyle fontStyle, FontWeight fontWeight, Brush foreground, Brush background, double fontSize)
        {


            if (keyword == " ")
            {
                TextRange selection = new TextRange(endPointer, endPointer);
                selection.ApplyPropertyValue(TextElement.ForegroundProperty, foreground);
            }

            FromTextPointer(startPointer, endPointer, keyword, fontStyle, fontWeight, foreground, background, fontSize, null);
        }

        /// <summary>
        /// Is manipulating a specific string inside of a TextPointer Range (TextBlock, TextBox...)
        /// </summary>
        /// <param name="startPointer">Starting point where to look</param>
        /// <param name="endPointer">Endpoint where to look</param>
        /// <param name="keyword">This is the string you want to manipulate</param>
        /// <param name="fontStyle">The new FontStyle</param>
        /// <param name="fontWeight">The new FontWeight</param>
        /// <param name="foreground">The new foreground</param>
        /// <param name="background">The new background</param>
        /// <param name="fontSize">The new FontSize</param>
        /// <param name="newString">The New String (if you want to replace, can be null)</param>
        public static void FromTextPointer(TextPointer startPointer, TextPointer endPointer, string keyword, FontStyle fontStyle, FontWeight fontWeight, Brush foreground, Brush background, double fontSize, string newString)
        {
            if (startPointer == null) throw new ArgumentNullException(nameof(startPointer));
            if (endPointer == null) throw new ArgumentNullException(nameof(endPointer));
            if (string.IsNullOrEmpty(keyword)) throw new ArgumentNullException(keyword);


            TextRange text = new TextRange(startPointer, endPointer);
            TextPointer current = text.Start.GetInsertionPosition(LogicalDirection.Forward);
            while (current != null)
            {
                string textInRun = current.GetTextInRun(LogicalDirection.Forward);
                if (!string.IsNullOrWhiteSpace(textInRun))
                {
                    int index = textInRun.IndexOf(keyword);
                    if (index != -1)
                    {
                        TextPointer selectionStart = current.GetPositionAtOffset(index, LogicalDirection.Forward);
                        TextPointer selectionEnd = selectionStart.GetPositionAtOffset(keyword.Length, LogicalDirection.Forward);
                        TextRange selection = new TextRange(selectionStart, selectionEnd);

                        if (!string.IsNullOrEmpty(newString))
                            selection.Text = newString;

                        selection.ApplyPropertyValue(TextElement.FontSizeProperty, fontSize);
                        selection.ApplyPropertyValue(TextElement.FontStyleProperty, fontStyle);
                        selection.ApplyPropertyValue(TextElement.FontWeightProperty, fontWeight);
                        selection.ApplyPropertyValue(TextElement.ForegroundProperty, foreground);
                        selection.ApplyPropertyValue(TextElement.BackgroundProperty, background);
                    }
                }
                current = current.GetNextContextPosition(LogicalDirection.Forward);
            }
        }
    }
}
