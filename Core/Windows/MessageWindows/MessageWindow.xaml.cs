using System;
using System.Windows;
using System.Windows.Media.Imaging;

namespace Core.Windows.MessageWindows
{
    public enum MessageWindowType
    {
        ConfirmationWithYesNo = 0,
        ConfirmationWithYesNoCancel,
        Information,
        Error,
        Warning
    }
    public enum MessageWindowImage
    {
        Warning = 0,
        Question,
        Information,
        Error,
        None
    }
    public enum MessageWindowButton
    {
        OK = 0,
        OKCancel = 1,
        YesNoCancel = 3,
        YesNo = 4
    }

    public partial class MessageWindow : Window
    {
        public MessageWindow()
        {
            InitializeComponent();
        }

        static MessageWindow _messageBox;
        static MessageBoxResult _result = MessageBoxResult.No;

        public static MessageBoxResult Show
        (string caption, string msg, MessageWindowType type)
        {
            switch (type)
            {
                case MessageWindowType.ConfirmationWithYesNo:
                    return Show(caption, msg, MessageWindowButton.YesNo,
                    MessageWindowImage.Question);
                case MessageWindowType.ConfirmationWithYesNoCancel:
                    return Show(caption, msg, MessageWindowButton.YesNoCancel,
                    MessageWindowImage.Question);
                case MessageWindowType.Information:
                    return Show(caption, msg, MessageWindowButton.OK,
                    MessageWindowImage.Information);
                case MessageWindowType.Error:
                    return Show(caption, msg, MessageWindowButton.OK,
                    MessageWindowImage.Error);
                case MessageWindowType.Warning:
                    return Show(caption, msg, MessageWindowButton.OK,
                    MessageWindowImage.Warning);
                default:
                    return MessageBoxResult.No;
            }
        }

        public static MessageBoxResult Show(string msg, MessageWindowType type)
        {
            return Show(string.Empty, msg, type);
        }

        public static MessageBoxResult Show(string msg)
        {
            return Show(string.Empty, msg,
            MessageWindowButton.OK, MessageWindowImage.None);
        }

        public static MessageBoxResult Show
        (string caption, string text)
        {
            return Show(caption, text,
            MessageWindowButton.OK, MessageWindowImage.None);
        }

        public static MessageBoxResult Show
        (string caption, string text, MessageWindowButton button)
        {
            return Show(caption, text, button,
            MessageWindowImage.None);
        }

        public static MessageBoxResult Show
        (string caption, string text,
        MessageWindowButton button, MessageWindowImage image)
        {
            _messageBox = new MessageWindow
            { txtMsg = { Text = text }, MessageTitle = { Text = caption } };
            SetVisibilityOfButtons(button);
            SetImageOfMessageBox(image);
            _messageBox.ShowDialog();
            return _result;
        }

        private static void SetVisibilityOfButtons(MessageWindowButton button)
        {
            switch (button)
            {
                case MessageWindowButton.OK:
                    _messageBox.btnCancel.Visibility = Visibility.Collapsed;
                    _messageBox.btnNo.Visibility = Visibility.Collapsed;
                    _messageBox.btnYes.Visibility = Visibility.Collapsed;
                    _messageBox.btnOk.Focus();
                    break;
                case MessageWindowButton.OKCancel:
                    _messageBox.btnNo.Visibility = Visibility.Collapsed;
                    _messageBox.btnYes.Visibility = Visibility.Collapsed;
                    _messageBox.btnCancel.Focus();
                    break;
                case MessageWindowButton.YesNo:
                    _messageBox.btnOk.Visibility = Visibility.Collapsed;
                    _messageBox.btnCancel.Visibility = Visibility.Collapsed;
                    _messageBox.btnNo.Focus();
                    break;
                case MessageWindowButton.YesNoCancel:
                    _messageBox.btnOk.Visibility = Visibility.Collapsed;
                    _messageBox.btnCancel.Focus();
                    break;
                default:
                    break;
            }
        }
        private static void SetImageOfMessageBox(MessageWindowImage image)
        {
            switch (image)
            {
                case MessageWindowImage.Warning:
                    _messageBox.SetImage("Warning.png");
                    break;
                case MessageWindowImage.Question:
                    _messageBox.SetImage("Question.png");
                    break;
                case MessageWindowImage.Information:
                    _messageBox.SetImage("Information.png");
                    break;
                case MessageWindowImage.Error:
                    _messageBox.SetImage("Error.png");
                    break;
                default:
                    _messageBox.img.Visibility = Visibility.Collapsed;
                    break;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (sender == btnOk)
                _result = MessageBoxResult.OK;
            else if (sender == btnYes)
                _result = MessageBoxResult.Yes;
            else if (sender == btnNo)
                _result = MessageBoxResult.No;
            else if (sender == btnCancel)
                _result = MessageBoxResult.Cancel;
            else
                _result = MessageBoxResult.None;

            _messageBox.Close();
            _messageBox = null;
        }

        private void SetImage(string imageName)
        {
            string uri = string.Format("/Images/{0}", imageName);
            var uriSource = new Uri(uri, UriKind.RelativeOrAbsolute);
            img.Source = new BitmapImage(uriSource);
        }

        private void Rectangle_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            this.DragMove();
        }
    }
}
