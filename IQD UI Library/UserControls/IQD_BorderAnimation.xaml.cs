using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace IQD_UI_Library.UserControls
{
    /// <summary>
    /// Interaction logic for IQDBorderAnimation.xaml
    /// </summary>
    public partial class IQDBorderAnimation : UserControl
    {
        public IQDBorderAnimation()
        {
            InitializeComponent();
        }


        // تعريف الخصائص القابلة للتخصيص
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(IQDBorderAnimation), new PropertyMetadata(""));

        public static readonly DependencyProperty TextColorProperty =
            DependencyProperty.Register("TextColor", typeof(Brush), typeof(IQDBorderAnimation), new PropertyMetadata(Brushes.White));

        public static readonly DependencyProperty BackgroundColorProperty =
            DependencyProperty.Register("BackgroundColor", typeof(Brush), typeof(IQDBorderAnimation), new PropertyMetadata(Brushes.Transparent));

        public static readonly DependencyProperty IconSourceProperty =
            DependencyProperty.Register("IconSource", typeof(ImageSource), typeof(IQDBorderAnimation), new PropertyMetadata(null));

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public Brush TextColor
        {
            get { return (Brush)GetValue(TextColorProperty); }
            set { SetValue(TextColorProperty, value); }
        }

        public Brush BackgroundColor
        {
            get { return (Brush)GetValue(BackgroundColorProperty); }
            set { SetValue(BackgroundColorProperty, value); }
        }

        public ImageSource IconSource
        {
            get { return (ImageSource)GetValue(IconSourceProperty); }
            set { SetValue(IconSourceProperty, value); }
        }

    }
}
