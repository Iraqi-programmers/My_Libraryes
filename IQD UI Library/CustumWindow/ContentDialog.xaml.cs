using System.Windows;
using System.Windows.Controls;

namespace IQD_UI_Library.CustumWindow
{
    /// <summary>
    /// Interaction logic for ContentDialog.xaml
    /// </summary>
    public partial class ContentDialog : Window
    {
        public ContentDialog()
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

        //private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        //{
        //    if (e.LeftButton == MouseButtonState.Pressed)
        //    {
        //        brdrOk.Opacity = 0.7;
        //        DragMove();
        //    }

        //    if (e.LeftButton == MouseButtonState.Released)
        //    {
        //        brdrOk.Opacity = 1;
        //    }

        //}

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
