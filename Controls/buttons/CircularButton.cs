using System.Windows;
using System.Windows.Controls;

namespace FootballApp.Controls.buttons
{
    public class CircularButton : Button
    {
        /// <summary>
        /// Unicode glyph text
        /// </summary>
        public string IconGlyph
        {
            get { return (string)GetValue(IconGlyphProperty); }
            set { SetValue(IconGlyphProperty, value); }
        }

        public static readonly DependencyProperty IconGlyphProperty =
            DependencyProperty.Register("IconGlyph", typeof(string), typeof(CircularButton), new PropertyMetadata(defaultValue: null));

    }
}
