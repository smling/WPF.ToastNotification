using System;
using System.Diagnostics;
using System.Windows;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;

namespace WPF.ToastNotification
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const string APP_ID = "WPF ToastNotificaiton Test";

        public MainWindow()
        {
            InitializeComponent();
        }


        private void btnSend_Click(object sender, RoutedEventArgs e)
        {
            var toastXml = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastImageAndText02);

            var textFields = toastXml.GetElementsByTagName("text");
            textFields[0].AppendChild(toastXml.CreateTextNode(txtTitle.Text));
            textFields[1].AppendChild(toastXml.CreateTextNode(txtContent.Text));

            String imagePath = txtIconPath.Text;

            XmlNodeList imageElements = toastXml.GetElementsByTagName("image");
            imageElements[0].Attributes.GetNamedItem("src").NodeValue = imagePath;

            Windows.UI.Notifications.ToastNotification toast = new Windows.UI.Notifications.ToastNotification(toastXml);
            toast.Activated += toast_Activated;
            toast.Dismissed += toast_Dismissed;
            toast.Failed += toast_Failed;

            // You must specifiy AppUserModelId == APP_ID to send toast notification.
            ToastNotificationManager.CreateToastNotifier(APP_ID).Show(toast);

            //lblStatus.Text = "Toast Sent!";
        }

        private void btnIconPath_Click(object sender, RoutedEventArgs e)
        {
            // Create OpenFileDialog 
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

            // Set filter for file extension and default file extension 
            dlg.DefaultExt = ".png";
            dlg.Filter = "JPEG Files (*.jpeg)|*.jpeg|PNG Files (*.png)|*.png|JPG Files (*.jpg)|*.jpg|GIF Files (*.gif)|*.gif";

            // Display OpenFileDialog by calling ShowDialog method 
            Nullable<bool> result = dlg.ShowDialog();

            // Get the selected file name and display in a TextBox 
            if (result == true)
            {
                // Open document 
                string filename = dlg.FileName;
                txtIconPath.Text = filename;
            }
        }

        private void toast_Failed(Windows.UI.Notifications.ToastNotification sender, ToastFailedEventArgs args)
        {
            Dispatcher.Invoke(() =>
            {
               MessageBox.Show(string.Format("Toast Failed - Error Code: {0}", args.ErrorCode.Message));
            });
        }

        private void toast_Dismissed(Windows.UI.Notifications.ToastNotification sender, ToastDismissedEventArgs args)
        {
            Dispatcher.Invoke(() =>
            {
                MessageBox.Show(args.Reason.ToString());
            });
        }

        private void toast_Activated(Windows.UI.Notifications.ToastNotification sender, object args)
        {
            Dispatcher.Invoke(() =>
            {
                Activate();
                Process.Start("http://www.chunho-ling.com");
            });
        }
    }
}
