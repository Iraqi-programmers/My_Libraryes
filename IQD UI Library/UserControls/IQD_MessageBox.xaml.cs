using System.Windows;

namespace IQD_UI_Library
{
    /// <summary>
    /// Interaction logic for IQD_MessageBox.xaml
    /// </summary>
    public partial class IQD_MessageBox : Window
    {
        public IQD_MessageBox()
        {
            InitializeComponent();
            DataContext = this; // ربط البيانات

        }

        // خصائص قابلة للربط
        public static readonly DependencyProperty BoxTitleProperty =
            DependencyProperty.Register("BoxTitle", typeof(string), typeof(IQD_MessageBox), new PropertyMetadata("BoxTitle"));

        public static readonly DependencyProperty MessageProperty =
            DependencyProperty.Register("Message", typeof(string), typeof(IQD_MessageBox), new PropertyMetadata("Message"));

        public static readonly DependencyProperty TypeProperty =
            DependencyProperty.Register("Type", typeof(MessageBoxType), typeof(IQD_MessageBox), new PropertyMetadata(MessageBoxType.Info));

        // BoxTitle
        public string BoxTitle
        {
            get { return (string)GetValue(BoxTitleProperty); }
            set { SetValue(BoxTitleProperty, value); }
        }

        // Message
        public string Message
        {
            get { return (string)GetValue(MessageProperty); }
            set { SetValue(MessageProperty, value); }
        }

        // Type
        public MessageBoxType Type
        {
            get { return (MessageBoxType)GetValue(TypeProperty); }
            set
            {
                SetValue(TypeProperty, value);
                UpdateButtonsVisibility();
            }
        }


        private void UpdateButtonsVisibility()
        {
            if (Type == MessageBoxType.Question)
            {
                YesButton.Visibility = Visibility.Visible;
                NoButton.Visibility = Visibility.Visible;
                CloseButton.Visibility = Visibility.Collapsed;
            }
            else
            {
                YesButton.Visibility = Visibility.Collapsed;
                NoButton.Visibility = Visibility.Collapsed;
                CloseButton.Visibility = Visibility.Visible;
            }
        }

        // نتيجة الاختيار
        public bool? Result { get; private set; }

        // حدث النقر على زر نعم
        private void YesButton_Click(object sender, RoutedEventArgs e)
        {
            Result = true;
            CloseWindow();
        }

        // حدث النقر على زر لا
        private void NoButton_Click(object sender, RoutedEventArgs e)
        {
            Result = false;
            CloseWindow();
        }


        // إغلاق النافذة
        private void CloseWindow()
        {
            var parentWindow = Window.GetWindow(this); // الحصول على النافذة الأم
            parentWindow.Close(); // إغلاق النافذة
        }

        // دالة لعرض الرسالة
        public static bool? Show(string title, string message, MessageBoxType type = MessageBoxType.Info)
        {
            var messageBoxControl = new IQD_MessageBox
            {
                BoxTitle = title,
                Message = message,
                Type = type
                ,
                WindowState = WindowState.Normal,
                WindowStyle = WindowStyle.None,
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                ResizeMode = ResizeMode.NoResize

            };

            messageBoxControl.ShowDialog(); // عرض النافذة كـ Dialog
            return messageBoxControl.Result; // إرجاع النتيجة
        }

        private void CloseBtn_Click(object sender, RoutedEventArgs e)
        {
            Result = null; // إذا كان النوع ليس سؤالًا
            CloseWindow();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Result = null; // إذا كان النوع ليس سؤالًا
            CloseWindow();
        }


    }
    // أنواع الرسائل
    public enum MessageBoxType
    {
        Info,
        Warning,
        Error,
        Question
    }


}

