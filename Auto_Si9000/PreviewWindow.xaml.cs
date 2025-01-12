using System.Windows;
using System.Windows.Media.Imaging;

namespace Auto_Si9000
{
    public partial class PreviewWindow : Window
    {
        public PreviewWindow(BitmapSource imageSource)
        {
            InitializeComponent();
            PreviewImage.Source = imageSource;
        }

        private void CopyImage_Click(object sender, RoutedEventArgs e)
        {
            if (PreviewImage.Source != null)
            {
                Clipboard.SetImage((BitmapSource)PreviewImage.Source);
                MessageBox.Show("图片已复制到剪贴板", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}