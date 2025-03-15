using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace IQD_UI_Library.UserControls
{
    public partial class IQD_FinancialAnalyzerCard : UserControl
    {
        public static readonly DependencyProperty StartPointProperty =
            DependencyProperty.Register("StartPoint", typeof(Point), typeof(IQD_FinancialAnalyzerCard), new PropertyMetadata(new Point(0, 0)));

        public static readonly DependencyProperty EndPointProperty =
            DependencyProperty.Register("EndPoint", typeof(Point), typeof(IQD_FinancialAnalyzerCard), new PropertyMetadata(new Point(1, 1)));

        public static readonly DependencyProperty GradientColor1Property = 
            DependencyProperty.Register("GradientColor1", typeof(Color), typeof(IQD_FinancialAnalyzerCard),
            new PropertyMetadata(Colors.LightBlue)); 

        public static readonly DependencyProperty GradientColor2Property =
            DependencyProperty.Register("GradientColor2", typeof(Color), typeof(IQD_FinancialAnalyzerCard),
            new PropertyMetadata(Colors.DodgerBlue)); 

        public Point StartPoint
        {
            get { return (Point)GetValue(StartPointProperty); }
            set { SetValue(StartPointProperty, value); }
        }

        public Point EndPoint
        {
            get { return (Point)GetValue(EndPointProperty); }
            set { SetValue(EndPointProperty, value); }
        }

        public Color GradientColor1
        {
            get { return (Color)GetValue(GradientColor1Property); }
            set { SetValue(GradientColor1Property, value); }
        }

        public Color GradientColor2
        {
            get { return (Color)GetValue(GradientColor2Property); }
            set { SetValue(GradientColor2Property, value); }
        }

        public IQD_FinancialAnalyzerCard()
        {
            InitializeComponent();
        }

        private void OpenFinancialAnalyzer(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show("تم فتح التحليل المالي الذكي!");
        }

        private void OnMouseEnter(object sender, MouseEventArgs e)
        {
            Border border = sender as Border;
            if (border != null)
            {
                border.RenderTransform = new ScaleTransform(1.05, 1.05);
            }
        }

        private void OnMouseLeave(object sender, MouseEventArgs e)
        {
            Border border = sender as Border;
            if (border != null)
            {
                border.RenderTransform = new ScaleTransform(1, 1);
            }
        }
    }
}
