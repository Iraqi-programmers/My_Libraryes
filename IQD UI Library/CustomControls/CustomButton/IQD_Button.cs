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
    ///     xmlns:MyNamespace="clr-namespace:IQD_UI_Library"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:IQD_UI_Library;assembly=IQD_UI_Library"
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
    ///     <MyNamespace:MyButton/>
    ///
    /// </summary>
    public class IQD_Button : Button
    {
        static IQD_Button() 
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(IQD_Button), new FrameworkPropertyMetadata(typeof(IQD_Button)));
        }
        

        // خاصية لتحديد الاستايل الخارجي
        public Style ExternalStyle
        {
            get => (Style)GetValue(ExternalStyleProperty);
            set => SetValue(ExternalStyleProperty, value);
        }

        public static readonly DependencyProperty ExternalStyleProperty =
            DependencyProperty.Register(nameof(ExternalStyle), typeof(Style), typeof(IQD_Button),
                new PropertyMetadata(null, OnExternalStyleChanged));

        // خاصية مصدر الصورة
        public ImageSource Icon
        {
            get => (ImageSource)GetValue(IconProperty);
            set => SetValue(IconProperty, value);
        }

        public static readonly DependencyProperty IconProperty =
            DependencyProperty.Register(nameof(Icon), typeof(ImageSource), typeof(IQD_Button), new PropertyMetadata(null));


        // خاصية لتحديد موضع الصورة (يمين، يسار، أعلى، أسفل)
        public Dock IconPosition
        {
            get => (Dock)GetValue(IconPositionProperty);
            set => SetValue(IconPositionProperty, value);
        }

        public static readonly DependencyProperty IconPositionProperty =
            DependencyProperty.Register(nameof(IconPosition), typeof(Dock), typeof(IQD_Button), new PropertyMetadata(Dock.Left));


        // خاصية لتحديد حجم الأيقونة
        public double IconSize
        {
            get { return (double)GetValue(IconSizeProperty); }
            set { SetValue(IconSizeProperty, value); }
        }

        public static readonly DependencyProperty IconSizeProperty =
            DependencyProperty.Register(nameof(IconSize), typeof(double), typeof(IQD_Button), new PropertyMetadata(24.0));



        private static void OnExternalStyleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is IQD_Button button && e.NewValue is Style newStyle)
            {
                button.Style = newStyle;
            }
        }
    }
}

