using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace IQD_UI_Library.UserControls
{
    /// <summary>
    /// Interaction logic for IQD_FinancialAnalyzerCard.xaml
    /// </summary>
    public partial class IQD_FinancialAnalyzerCard : UserControl
    {
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
