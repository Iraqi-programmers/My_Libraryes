using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace IQD_UI_Library
{
    public class IQD_TextBoxValidation : TextBox, INotifyDataErrorInfo
    {
        static IQD_TextBoxValidation()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(IQD_TextBoxValidation),
                new FrameworkPropertyMetadata(typeof(IQD_TextBoxValidation)));
        }

        #region Dependency Properties

        public static readonly DependencyProperty InnerBorderImageProperty =
            DependencyProperty.Register(nameof(InnerBorderImage), typeof(ImageSource), typeof(IQD_TextBoxValidation));

        public static new readonly DependencyProperty BorderBrushProperty =
            DependencyProperty.Register(nameof(BorderBrush), typeof(Brush), typeof(IQD_TextBoxValidation));

        public static readonly DependencyProperty ErrorTextColorProperty =
            DependencyProperty.Register(nameof(ErrorTextColor), typeof(Brush), typeof(IQD_TextBoxValidation));

        public static readonly DependencyProperty ErrorBrushProperty =
            DependencyProperty.Register(nameof(ErrorBorderBrush), typeof(Brush), typeof(IQD_TextBoxValidation), new PropertyMetadata(Brushes.Red));

        public static readonly DependencyProperty FocusedBorderBrushProperty =
            DependencyProperty.Register(nameof(FocusedBorderBrush), typeof(Brush), typeof(IQD_TextBoxValidation), new PropertyMetadata(Brushes.Blue));

        public static readonly DependencyProperty ValidationTypeProperty =
            DependencyProperty.Register(nameof(ValidationType), typeof(ValidationType), typeof(IQD_TextBoxValidation),
                new PropertyMetadata(ValidationType.None, OnValidationTypeChanged));

        public static readonly DependencyProperty ErrorMessageProperty =
            DependencyProperty.Register(nameof(ErrorMessage), typeof(string), typeof(IQD_TextBoxValidation),
                new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public static readonly DependencyProperty HasErrorProperty =
            DependencyProperty.Register(nameof(HasError), typeof(bool), typeof(IQD_TextBoxValidation));

        public static readonly DependencyProperty PlaceholderTextProperty =
            DependencyProperty.Register(nameof(PlaceholderText), typeof(string), typeof(IQD_TextBoxValidation));

        public static readonly DependencyProperty IsTouchedProperty =
    DependencyProperty.Register(nameof(IsTouched), typeof(bool), typeof(IQD_TextBoxValidation), new PropertyMetadata(false));

        public static readonly DependencyProperty InnerIconAlignmentProperty =
    DependencyProperty.Register(nameof(InnerIconAlignment), typeof(IconAlignment), typeof(IQD_TextBoxValidation),
        new PropertyMetadata(IconAlignment.Right));



        #endregion

        #region CLR Properties

        public bool IsTouched
        {
            get => (bool)GetValue(IsTouchedProperty);
            set => SetValue(IsTouchedProperty, value);
        }

        public ImageSource InnerBorderImage
        {
            get => (ImageSource)GetValue(InnerBorderImageProperty);
            set => SetValue(InnerBorderImageProperty, value);
        }

        public new Brush BorderBrush
        {
            get => (Brush)GetValue(BorderBrushProperty);
            set => SetValue(BorderBrushProperty, value);
        }

        public Brush ErrorTextColor
        {
            get => (Brush)GetValue(ErrorTextColorProperty);
            set => SetValue(ErrorTextColorProperty, value);
        }

        public Brush ErrorBorderBrush
        {
            get => (Brush)GetValue(ErrorBrushProperty);
            set => SetValue(ErrorBrushProperty, value);
        }

        public Brush FocusedBorderBrush
        {
            get => (Brush)GetValue(FocusedBorderBrushProperty);
            set => SetValue(FocusedBorderBrushProperty, value);
        }

        public ValidationType ValidationType
        {
            get => (ValidationType)GetValue(ValidationTypeProperty);
            set => SetValue(ValidationTypeProperty, value);
        }

        public string ErrorMessage
        {
            get => (string)GetValue(ErrorMessageProperty);
            set => SetValue(ErrorMessageProperty, value);
        }

        public bool HasError
        {
            get => (bool)GetValue(HasErrorProperty);
            set => SetValue(HasErrorProperty, value);
        }

        public string PlaceholderText
        {
            get => (string)GetValue(PlaceholderTextProperty);
            set => SetValue(PlaceholderTextProperty, value);
        }

        public IconAlignment InnerIconAlignment
        {
            get => (IconAlignment)GetValue(InnerIconAlignmentProperty);
            set => SetValue(InnerIconAlignmentProperty, value);
        }

        #endregion

        #region Validation Logic

        private static void OnValidationTypeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is IQD_TextBoxValidation textBox)
            {
                textBox.ValidateText();
            }
        }

        protected override void OnTextChanged(TextChangedEventArgs e)
        {
            base.OnTextChanged(e);
            ValidateText();
        }

        protected override void OnLostFocus(RoutedEventArgs e)
        {
            base.OnLostFocus(e);
            IsTouched = true;
            ValidateText();
        }

        private void ValidateText()
        {
            if(!IsTouched)
                return;
            ClearErrors();

            if (ValidationType.HasFlag(ValidationType.Required) && string.IsNullOrWhiteSpace(Text))
            {
                SetError(ErrorMessage ?? "هذا الحقل مطلوب");
                return;
            }

            if (ValidationType.HasFlag(ValidationType.Numeric) && !string.IsNullOrWhiteSpace(Text) && !double.TryParse(Text, out _))
            {
                SetError(ErrorMessage ?? "يجب إدخال رقم صحيح");
                return;
            }

            if (ValidationType.HasFlag(ValidationType.Email) && !string.IsNullOrWhiteSpace(Text) &&
                !System.Text.RegularExpressions.Regex.IsMatch(Text, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                SetError(ErrorMessage ?? "صيغة البريد الإلكتروني غير صحيحة");
                return;
            }

            if (ValidationType.HasFlag(ValidationType.Phone) && !string.IsNullOrWhiteSpace(Text) &&
                !System.Text.RegularExpressions.Regex.IsMatch(Text, @"^(\+?\d{1,3})?\d{10,}$"))
            {
                SetError(ErrorMessage ?? "رقم الهاتف غير صحيح");
                return;
            }
        }


        private void SetError(string error)
        {
            HasError = true;
            SetCurrentValue(ErrorMessageProperty, error);
            RaiseErrorsChanged();
        }

        private void ClearErrors()
        {
            HasError = false;
            if (!string.IsNullOrEmpty(ErrorMessage))
                SetCurrentValue(ErrorMessageProperty, null);
            RaiseErrorsChanged();
        }

        #endregion

        #region INotifyDataErrorInfo

        public bool HasErrors => HasError;

        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

        public IEnumerable? GetErrors(string propertyName)
        {
            if (propertyName == nameof(Text) && HasError)
            {
                return new List<string> { ErrorMessage };
            }
            return null;
        }

        private void RaiseErrorsChanged()
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(Text)));
        }

        #endregion
    }

    [Flags]
    public enum ValidationType
    {
        None = 0,
        Required = 1,
        Numeric = 2,
        Email = 4,
        Phone = 8
    }

    public enum IconAlignment
    {
        Left,
        Right
    }

    public class StringToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return string.IsNullOrEmpty(value as string) ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class ImageSourceToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value != null ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class ErrorAndTouchedToVisibilityConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length == 2 &&
                values[0] is bool hasError &&
                values[1] is bool isTouched)
            {
                return (hasError && isTouched) ? Visibility.Visible : Visibility.Collapsed;
            }
            return Visibility.Collapsed;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }


}
