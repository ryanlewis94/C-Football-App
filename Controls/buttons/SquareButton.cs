using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace FootballApp.Controls.buttons
{
    class SquareButton : Button
    {
        /// <summary>
        /// Text contained inside the button
        /// </summary>
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(SquareButton), new PropertyMetadata(defaultValue: null));


        /// <summary>
        /// Unicode glyph text
        /// </summary>
        public string IconGlyph
        {
            get { return (string)GetValue(IconGlyphProperty); }
            set { SetValue(IconGlyphProperty, value); }

        }

        public static readonly DependencyProperty IconGlyphProperty =
            DependencyProperty.Register("IconGlyph", typeof(string),
                typeof(SquareButton),
                new PropertyMetadata(defaultValue: null));


        /// <summary>
        /// Is this a negative button e.g. Reject / Cancel / No
        /// </summary>
        public bool IsNegativeButton
        {
            get { return (bool)GetValue(IsNegativeButtonProperty); }
            set { SetValue(IsNegativeButtonProperty, value); }
        }

        public static readonly DependencyProperty IsNegativeButtonProperty =
            DependencyProperty.Register("IsNegativeButton", typeof(bool), typeof(SquareButton), new PropertyMetadata(false));
    }
}
