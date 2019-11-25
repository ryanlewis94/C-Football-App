using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace FootballApp.Controls.buttons
{
    public class PlainIconButton : Button
    {
        public PlainIconButton()
        {
            IsInteractive = true;
        }

        /// <summary>
        /// The unicode text for the icon
        /// </summary>
        public string IconGlyph
        {
            get { return (string)GetValue(IconGlyphProperty); }
            set { SetValue(IconGlyphProperty, value); }
        }

        public static readonly DependencyProperty IconGlyphProperty =
            DependencyProperty.Register("IconGlyph",
                typeof(string),
                typeof(PlainIconButton),
                new PropertyMetadata(defaultValue: null));

        /// <summary>
        /// If the icon's opacity should animate on mouse over and mouse down events. True by default
        /// </summary>
        public bool IsInteractive
        {
            get { return (bool)GetValue(IsInteractiveProperty); }
            set { SetValue(IsInteractiveProperty, value); }
        }

        public static readonly DependencyProperty IsInteractiveProperty =
            DependencyProperty.Register("IsInteractive",
                typeof(bool),
                typeof(PlainIconButton),
                new PropertyMetadata(defaultValue: false));


        public SolidColorBrush IconBrush
        {
            get { return (SolidColorBrush)GetValue(IconBrushProperty); }
            set { SetValue(IconBrushProperty, value); }
        }

        public static readonly DependencyProperty IconBrushProperty =
            DependencyProperty.Register("IconBrush",
                typeof(SolidColorBrush),
                typeof(PlainIconButton),
                new PropertyMetadata());


        /// <summary>
        /// Some Icons are more visible with a white background such as with the warning icon
        /// </summary>
        public bool IsWhiteBackgroundRequired
        {
            get { return (bool)GetValue(IsWhiteBackgroundRequiredProperty); }
            set { SetValue(IsWhiteBackgroundRequiredProperty, value); }
        }

        public static readonly DependencyProperty IsWhiteBackgroundRequiredProperty =
            DependencyProperty.Register("IsWhiteBackgroundRequired", typeof(bool), typeof(PlainIconButton), new PropertyMetadata(false));
    }
}
