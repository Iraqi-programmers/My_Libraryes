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
using System.Windows.Shapes;

namespace IQD_UI_Library
{
    /// <summary>
    /// Interaction logic for IQD_ContentDilog.xaml
    /// </summary>
    public partial class IQD_ContentDilog : Window
    {
        public IQD_ContentDilog()
        {
            InitializeComponent();
        }
        public UserControl Content
        {
            get;
            set;
        }


        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }


        private void ContentFrame_Loaded(object sender, RoutedEventArgs e)
        {
            if (this.Content != null)
            {
                ContentFrame.Content = this.Content;
            }
            else
            {
                ContentFrame.Content = null;

            }

        }

    }
}
