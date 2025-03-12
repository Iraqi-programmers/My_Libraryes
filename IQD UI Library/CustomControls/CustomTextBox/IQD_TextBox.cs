using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace IQD_UI_Library
{
    /// <summary>
    /// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
    ///
    /// Step 1a) Using this custom control in a XAML file that exists in the current project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:IQD_UI_Library.Custom_Controls.Custom_TextBox"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:IQD_UI_Library.Custom_Controls.Custom_TextBox;assembly=IQD_UI_Library.Custom_Controls.Custom_TextBox"
    ///
    /// You will also need to add a project reference from the project where the XAML file lives
    /// to this project and Rebuild to avoid compilation errors:
    ///
    ///     Right click on the target project in the Solution Explorer and
    ///     "Add Reference"->"Projects"->[Browse to and select this project]
    ///
    ///
    /// Step 2)
    /// Go ahead and use your control in the XAML file.
    ///
    ///     <MyNamespace:IQD_TextBox/>
    ///
    /// </summary>
    public class IQD_TextBox : TextBox
    {
        static IQD_TextBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(IQD_TextBox), new FrameworkPropertyMetadata(typeof(IQD_TextBox)));
        }

        public static readonly DependencyProperty InnerBorderImageProperty =
        DependencyProperty.Register("InnerBorderImage", typeof(ImageSource), typeof(IQD_TextBox), new PropertyMetadata(null));


        public static readonly DependencyProperty BorderBrushing =
       DependencyProperty.Register("BorderBrushing", typeof(Brush), typeof(IQD_TextBox), new PropertyMetadata(null));

        public Brush BorderBrushes
        {
            get { return (Brush)GetValue(BorderBrushing); }
            set { SetValue(BorderBrushing, value); }
        }
        public ImageSource InnerBorderImage
        {
            get { return (ImageSource)GetValue(InnerBorderImageProperty); }
            set { SetValue(InnerBorderImageProperty, value); }
        }

    }
}
