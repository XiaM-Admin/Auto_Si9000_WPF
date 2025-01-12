using System.Collections.ObjectModel;
using System.Windows;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using Point = System.Windows.Point;
using FontFamily = System.Windows.Media.FontFamily;

namespace Auto_Si9000
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ObservableCollection<外层单线不对地_Model> _tab1DataGridItemsSource =
            new ObservableCollection<外层单线不对地_Model>
            {
                new 外层单线不对地_Model { H1 = 0.077, Er1 = 3.9, W1=0.120, T1=0.045 }
            };

        private ObservableCollection<外层单线对地_Model> _tab2DataGridItemsSource =
           new ObservableCollection<外层单线对地_Model>
           {
                new 外层单线对地_Model { H1 = 0.077, Er1 = 3.9, W1=0.1, D1=0.11, T1=0.045 }
           };

        private ObservableCollection<内层单线不对地_Model> _tab3DataGridItemsSource =
           new ObservableCollection<内层单线不对地_Model>
           {
                new 内层单线不对地_Model { H1 = 0.83, Er1 = 4.4, H2=0.112, Er2=3.9, W1=0.11, T1=0.03 }
           };

        private ObservableCollection<内层单线对地_Model> _tab4DataGridItemsSource =
           new ObservableCollection<内层单线对地_Model>
           {
                new 内层单线对地_Model { H1 = 0.83, Er1 = 4.4, H2=0.112, Er2=3.9, W1=0.095, D1=0.128, T1=0.03 }
           };

        private ObservableCollection<外层双线不对地_Model> _tab5DataGridItemsSource =
           new ObservableCollection<外层双线不对地_Model>
           {
                new 外层双线不对地_Model { H1 = 0.077, Er1 = 3.9, W1=0.122, S1=0.182, T1=0.045, C1=0.04, C2=0.012, C3=0.04, CEr=3.5 }
           };

        private ObservableCollection<外层双线对地_Model> _tab6DataGridItemsSource =
           new ObservableCollection<外层双线对地_Model>
           {
                new 外层双线对地_Model { H1 = 0.077, Er1 = 3.9, W1=0.118, S1=0.172, D1=0.161, T1=0.045, C1=0.04, C2=0.012, C3=0.04, CEr=3.5 }
           };

        private ObservableCollection<内层双线不对地_Model> _tab7DataGridItemsSource =
           new ObservableCollection<内层双线不对地_Model>
           {
                new 内层双线不对地_Model {  H1 = 0.83, Er1 = 4.4, H2=0.112, Er2=3.9, W1=0.103, S1=0.167, T1=0.03 }
           };

        private ObservableCollection<内层双线对地_Model> _tab8DataGridItemsSource =
           new ObservableCollection<内层双线对地_Model>
           {
                new 内层双线对地_Model {  H1 = 0.83, Er1 = 4.4, H2=0.112, Er2=3.9, W1=0.092, S1=0.138, D1=0.154, T1=0.03 }
           };

        public MainWindow()
        {
            InitializeComponent();
            Tab1DataGrid.ItemsSource = _tab1DataGridItemsSource;
            Tab2DataGrid.ItemsSource = _tab2DataGridItemsSource;
            Tab3DataGrid.ItemsSource = _tab3DataGridItemsSource;
            Tab4DataGrid.ItemsSource = _tab4DataGridItemsSource;
            Tab5DataGrid.ItemsSource = _tab5DataGridItemsSource;
            Tab6DataGrid.ItemsSource = _tab6DataGridItemsSource;
            Tab7DataGrid.ItemsSource = _tab7DataGridItemsSource;
            Tab8DataGrid.ItemsSource = _tab8DataGridItemsSource;
        }

        /// <summary>
        /// 外层单线不对地正算
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Tab1zsButton_Click(object sender, RoutedEventArgs e)
        {
            string Warring_Message = "";
            foreach (var item in _tab1DataGridItemsSource)
            {
                if (item.Check_Valid())
                    item.Calculate_Zo();
                else
                {
                    Warring_Message += "第" + (_tab1DataGridItemsSource.IndexOf(item) + 1) + "行数据有误或为空，请检查！\n";
                    continue;
                }
            }
            Tab1DataGrid.ItemsSource = null;
            Tab1DataGrid.ItemsSource = _tab1DataGridItemsSource;
            if (Warring_Message is not "")
                MessageBox.Show(Warring_Message, "警告", MessageBoxButton.OK, MessageBoxImage.Warning);
            else
                MessageBox.Show($"正算完成！\r\n一共{_tab1DataGridItemsSource.Count}行数据。", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        /// <summary>
        /// 外层单线不对地反算
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Tab1fsButton_Click(object sender, RoutedEventArgs e)
        {
            string Warring_Message = "";
            foreach (var item in _tab1DataGridItemsSource)
            {
                if (item.Check_Valid() && item.Check_TargetZo())
                    item.ResCalculate_Zo();
                else
                {
                    Warring_Message += "第" + (_tab1DataGridItemsSource.IndexOf(item) + 1) + "行数据有误或为空，请检查！\n";
                    continue;
                }
            }
            Tab1DataGrid.ItemsSource = null;
            Tab1DataGrid.ItemsSource = _tab1DataGridItemsSource;
            if (Warring_Message is not "")
                MessageBox.Show(Warring_Message, "警告", MessageBoxButton.OK, MessageBoxImage.Warning);
            else
                MessageBox.Show($"反算完成！\r\n一共{_tab1DataGridItemsSource.Count}行数据。", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        /// <summary>
        /// 外层单线对地正算
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Tab2zsButton_Click(object sender, RoutedEventArgs e)
        {
            string Warring_Message = "";
            foreach (var item in _tab2DataGridItemsSource)
            {
                if (item.Check_Valid())
                    item.Calculate_Zo();
                else
                {
                    Warring_Message += "第" + (_tab2DataGridItemsSource.IndexOf(item) + 1) + "行数据有误或为空，请检查！\n";
                    continue;
                }
            }
            Tab2DataGrid.ItemsSource = null;
            Tab2DataGrid.ItemsSource = _tab2DataGridItemsSource;
            if (Warring_Message is not "")
                MessageBox.Show(Warring_Message, "警告", MessageBoxButton.OK, MessageBoxImage.Warning);
            else
                MessageBox.Show($"正算完成！\r\n一共{_tab2DataGridItemsSource.Count}行数据。", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        /// <summary>
        /// 外层单线对地反算
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Tab2fsButton_Click(object sender, RoutedEventArgs e)
        {
            string Warring_Message = "";
            foreach (var item in _tab2DataGridItemsSource)
            {
                if (item.Check_Valid() && item.Check_TargetZo())
                    item.ResCalculate_Zo();
                else
                {
                    Warring_Message += "第" + (_tab2DataGridItemsSource.IndexOf(item) + 1) + "行数据有误或为空，请检查！\n";
                    continue;
                }
            }
            Tab2DataGrid.ItemsSource = null;
            Tab2DataGrid.ItemsSource = _tab2DataGridItemsSource;
            if (Warring_Message is not "")
                MessageBox.Show(Warring_Message, "警告", MessageBoxButton.OK, MessageBoxImage.Warning);
            else
                MessageBox.Show($"反算完成！\r\n一共{_tab2DataGridItemsSource.Count}行数据。", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        /// <summary>
        /// 内层单线不对地正算
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Tab3zsButton_Click(object sender, RoutedEventArgs e)
        {
            string Warring_Message = "";
            foreach (var item in _tab3DataGridItemsSource)
            {
                if (item.Check_Valid())
                    item.Calculate_Zo();
                else
                {
                    Warring_Message += "第" + (_tab3DataGridItemsSource.IndexOf(item) + 1) + "行数据有误或为空，请检查！\n";
                    continue;
                }
            }
            Tab3DataGrid.ItemsSource = null;
            Tab3DataGrid.ItemsSource = _tab3DataGridItemsSource;
            if (Warring_Message is not "")
                MessageBox.Show(Warring_Message, "警告", MessageBoxButton.OK, MessageBoxImage.Warning);
            else
                MessageBox.Show($"正算完成！\r\n一共{_tab3DataGridItemsSource.Count}行数据。", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        /// <summary>
        /// 内层单线不对地反算
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Tab3fsButton_Click(object sender, RoutedEventArgs e)
        {
            string Warring_Message = "";
            foreach (var item in _tab3DataGridItemsSource)
            {
                if (item.Check_Valid() && item.Check_TargetZo())
                    item.ResCalculate_Zo();
                else
                {
                    Warring_Message += "第" + (_tab3DataGridItemsSource.IndexOf(item) + 1) + "行数据有误或为空，请检查！\n";
                    continue;
                }
            }
            Tab3DataGrid.ItemsSource = null;
            Tab3DataGrid.ItemsSource = _tab3DataGridItemsSource;
            if (Warring_Message is not "")
                MessageBox.Show(Warring_Message, "警告", MessageBoxButton.OK, MessageBoxImage.Warning);
            else
                MessageBox.Show($"反算完成！\r\n一共{_tab3DataGridItemsSource.Count}行数据。", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        /// <summary>
        /// 内层单线对地zs
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Tab4zsButton_Click(object sender, RoutedEventArgs e)
        {
            string Warring_Message = "";
            foreach (var item in _tab4DataGridItemsSource)
            {
                if (item.Check_Valid())
                    item.Calculate_Zo();
                else
                {
                    Warring_Message += "第" + (_tab4DataGridItemsSource.IndexOf(item) + 1) + "行数据有误或为空，请检查！\n";
                    continue;
                }
            }
            Tab4DataGrid.ItemsSource = null;
            Tab4DataGrid.ItemsSource = _tab4DataGridItemsSource;
            if (Warring_Message is not "")
                MessageBox.Show(Warring_Message, "警告", MessageBoxButton.OK, MessageBoxImage.Warning);
            else
                MessageBox.Show($"正算完成！\r\n一共{_tab4DataGridItemsSource.Count}行数据。", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        /// <summary>
        /// 内层单线对地fs
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Tab4fsButton_Click(object sender, RoutedEventArgs e)
        {
            string Warring_Message = "";
            foreach (var item in _tab4DataGridItemsSource)
            {
                if (item.Check_Valid() && item.Check_TargetZo())
                    item.ResCalculate_Zo();
                else
                {
                    Warring_Message += "第" + (_tab4DataGridItemsSource.IndexOf(item) + 1) + "行数据有误或为空，请检查！\n";
                    continue;
                }
            }
            Tab4DataGrid.ItemsSource = null;
            Tab4DataGrid.ItemsSource = _tab4DataGridItemsSource;
            if (Warring_Message is not "")
                MessageBox.Show(Warring_Message, "警告", MessageBoxButton.OK, MessageBoxImage.Warning);
            else
                MessageBox.Show($"反算完成！\r\n一共{_tab4DataGridItemsSource.Count}行数据。", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        /// <summary>
        /// 外层双线不对地zs
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Tab5zsButton_Click(object sender, RoutedEventArgs e)
        {
            string Warring_Message = "";
            foreach (var item in _tab5DataGridItemsSource)
            {
                if (item.Check_Valid())
                    item.Calculate_Zo();
                else
                {
                    Warring_Message += "第" + (_tab5DataGridItemsSource.IndexOf(item) + 1) + "行数据有误或为空，请检查！\n";
                    continue;
                }
            }
            Tab5DataGrid.ItemsSource = null;
            Tab5DataGrid.ItemsSource = _tab5DataGridItemsSource;
            if (Warring_Message is not "")
                MessageBox.Show(Warring_Message, "警告", MessageBoxButton.OK, MessageBoxImage.Warning);
            else
                MessageBox.Show($"正算完成！\r\n一共{_tab5DataGridItemsSource.Count}行数据。", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        /// <summary>
        /// 外层双线不对地fs
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Tab5fsButton_Click(object sender, RoutedEventArgs e)
        {
            string Warring_Message = "";
            foreach (var item in _tab5DataGridItemsSource)
            {
                if (item.Check_Valid() && item.Check_TargetZo())
                    item.ResCalculate_Zo();
                else
                {
                    Warring_Message += "第" + (_tab5DataGridItemsSource.IndexOf(item) + 1) + "行数据有误或为空，请检查！\n";
                    continue;
                }
            }
            Tab5DataGrid.ItemsSource = null;
            Tab5DataGrid.ItemsSource = _tab5DataGridItemsSource;
            if (Warring_Message is not "")
                MessageBox.Show(Warring_Message, "警告", MessageBoxButton.OK, MessageBoxImage.Warning);
            else
                MessageBox.Show($"反算完成！\r\n一共{_tab5DataGridItemsSource.Count}行数据。", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        /// <summary>
        /// 外层双线对地zs
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Tab6zsButton_Click(object sender, RoutedEventArgs e)
        {
            string Warring_Message = "";
            foreach (var item in _tab6DataGridItemsSource)
            {
                if (item.Check_Valid())
                    item.Calculate_Zo();
                else
                {
                    Warring_Message += "第" + (_tab6DataGridItemsSource.IndexOf(item) + 1) + "行数据有误或为空，请检查！\n";
                    continue;
                }
            }
            Tab6DataGrid.ItemsSource = null;
            Tab6DataGrid.ItemsSource = _tab6DataGridItemsSource;
            if (Warring_Message is not "")
                MessageBox.Show(Warring_Message, "警告", MessageBoxButton.OK, MessageBoxImage.Warning);
            else
                MessageBox.Show($"正算完成！\r\n一共{_tab6DataGridItemsSource.Count}行数据。", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        /// <summary>
        /// 外层双线对地fs
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Tab6fsButton_Click(object sender, RoutedEventArgs e)
        {
            string Warring_Message = "";
            foreach (var item in _tab6DataGridItemsSource)
            {
                if (item.Check_Valid() && item.Check_TargetZo())
                    item.ResCalculate_Zo();
                else
                {
                    Warring_Message += "第" + (_tab6DataGridItemsSource.IndexOf(item) + 1) + "行数据有误或为空，请检查！\n";
                    continue;
                }
            }
            Tab6DataGrid.ItemsSource = null;
            Tab6DataGrid.ItemsSource = _tab6DataGridItemsSource;
            if (Warring_Message is not "")
                MessageBox.Show(Warring_Message, "警告", MessageBoxButton.OK, MessageBoxImage.Warning);
            else
                MessageBox.Show($"反算完成！\r\n一共{_tab6DataGridItemsSource.Count}行数据。", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        /// <summary>
        /// 内层双线不对地zs
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Tab7zsButton_Click(object sender, RoutedEventArgs e)
        {
            string Warring_Message = "";
            foreach (var item in _tab7DataGridItemsSource)
            {
                if (item.Check_Valid())
                    item.Calculate_Zo();
                else
                {
                    Warring_Message += "第" + (_tab7DataGridItemsSource.IndexOf(item) + 1) + "行数据有误或为空，请检查！\n";
                    continue;
                }
            }
            Tab7DataGrid.ItemsSource = null;
            Tab7DataGrid.ItemsSource = _tab7DataGridItemsSource;
            if (Warring_Message is not "")
                MessageBox.Show(Warring_Message, "警告", MessageBoxButton.OK, MessageBoxImage.Warning);
            else
                MessageBox.Show($"正算完成！\r\n一共{_tab7DataGridItemsSource.Count}行数据。", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        /// <summary>
        /// 内层双线不对地fs
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Tab7fsButton_Click(object sender, RoutedEventArgs e)
        {
            string Warring_Message = "";
            foreach (var item in _tab7DataGridItemsSource)
            {
                if (item.Check_Valid() && item.Check_TargetZo())
                    item.ResCalculate_Zo();
                else
                {
                    Warring_Message += "第" + (_tab7DataGridItemsSource.IndexOf(item) + 1) + "行数据有误或为空，请检查！\n";
                    continue;
                }
            }
            Tab7DataGrid.ItemsSource = null;
            Tab7DataGrid.ItemsSource = _tab7DataGridItemsSource;
            if (Warring_Message is not "")
                MessageBox.Show(Warring_Message, "警告", MessageBoxButton.OK, MessageBoxImage.Warning);
            else
                MessageBox.Show($"反算完成！\r\n一共{_tab7DataGridItemsSource.Count}行数据。", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        /// <summary>
        /// 内层双线对地zs
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Tab8zsButton_Click(object sender, RoutedEventArgs e)
        {
            string Warring_Message = "";
            foreach (var item in _tab8DataGridItemsSource)
            {
                if (item.Check_Valid())
                    item.Calculate_Zo();
                else
                {
                    Warring_Message += "第" + (_tab8DataGridItemsSource.IndexOf(item) + 1) + "行数据有误或为空，请检查！\n";
                    continue;
                }
            }
            Tab8DataGrid.ItemsSource = null;
            Tab8DataGrid.ItemsSource = _tab8DataGridItemsSource;
            if (Warring_Message is not "")
                MessageBox.Show(Warring_Message, "警告", MessageBoxButton.OK, MessageBoxImage.Warning);
            else
                MessageBox.Show($"正算完成！\r\n一共{_tab8DataGridItemsSource.Count}行数据。", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        /// <summary>
        /// 内层双线对地fs
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Tab8fsButton_Click(object sender, RoutedEventArgs e)
        {
            string Warring_Message = "";
            foreach (var item in _tab8DataGridItemsSource)
            {
                if (item.Check_Valid() && item.Check_TargetZo())
                    item.ResCalculate_Zo();
                else
                {
                    Warring_Message += "第" + (_tab8DataGridItemsSource.IndexOf(item) + 1) + "行数据有误或为空，请检查！\n";
                    continue;
                }
            }
            Tab8DataGrid.ItemsSource = null;
            Tab8DataGrid.ItemsSource = _tab8DataGridItemsSource;
            if (Warring_Message is not "")
                MessageBox.Show(Warring_Message, "警告", MessageBoxButton.OK, MessageBoxImage.Warning);
            else
                MessageBox.Show($"反算完成！\r\n一共{_tab8DataGridItemsSource.Count}行数据。", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        /// <summary>
        /// 外层单线不对地数据预览
        /// </summary>
        private void Tab1DataGrid_PreviewData(object sender, RoutedEventArgs e)
        {
            var selectedItem = Tab1DataGrid.SelectedItem as 外层单线不对地_Model;
            if (selectedItem != null)
            {
                selectedItem.Calculate_Zo();
                //selectedItem.Zo 重新刷新到源中，更新UI显示的数据
                Tab1DataGrid.Items.Refresh();
                try
                {
                    // 加载原始图片
                    var bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.UriSource = new Uri("pack://application:,,,/Assets/外层单线不对地-si9000.jpg");
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.EndInit();

                    // 创建可写的位图
                    var drawingVisual = new DrawingVisual();
                    using (DrawingContext drawingContext = drawingVisual.RenderOpen())
                    {
                        // 绘制原始图片
                        drawingContext.DrawImage(bitmap, new Rect(0, 0, bitmap.PixelWidth, bitmap.PixelHeight));

                        // 设置文本格式
                        var typeface = new Typeface(
                            new FontFamily("Segoe UI"),
                            FontStyles.Normal,
                            FontWeights.Normal,
                            FontStretches.Normal);
                        var brush = new SolidColorBrush(Colors.Black);

                        // 按特定顺序获取属性值，并计算W2
                        double w2 = ((selectedItem.W1 / 0.0254) - 1) * 0.0254;
                        var values = new List<string>
                        {
                            selectedItem.H1.ToString("F4"),
                            selectedItem.Er1.ToString("F4"),
                            selectedItem.W1.ToString("F4"),
                            w2.ToString("F4"),
                            selectedItem.T1.ToString("F4"),
                            selectedItem.C1.ToString("F4"),
                            selectedItem.C2.ToString("F4"),
                            selectedItem.CEr.ToString("F4"),
                            selectedItem.Zo.ToString("F2") ?? "0.00",
                        };

                        // 在指定坐标绘制文本
                        int[] yCoordinates = { 16, 40, 64, 88, 112, 136, 160, 184, 234 };
                        int[] xCoordinate = { 578, 578, 578, 578, 578, 578, 578, 578, 585 };
                        float fontSize = 13;

                        for (int i = 0; i < values.Count; i++)
                        {
                            if (!string.IsNullOrEmpty(values[i]))
                            {
                                var formattedText = new FormattedText(
                                    values[i],
                                    System.Globalization.CultureInfo.CurrentCulture,
                                    FlowDirection.LeftToRight,
                                    typeface,
                                    fontSize,
                                    brush,
                                    VisualTreeHelper.GetDpi(this).PixelsPerDip);

                                drawingContext.DrawText(formattedText, new Point(xCoordinate[i], yCoordinates[i]));
                            }
                        }
                    }

                    // 创建RenderTargetBitmap
                    var renderBitmap = new RenderTargetBitmap(
                        bitmap.PixelWidth, bitmap.PixelHeight,
                        96, 96, PixelFormats.Pbgra32);
                    renderBitmap.Render(drawingVisual);

                    // 显示预览窗口
                    var previewWindow = new PreviewWindow(renderBitmap);
                    previewWindow.ShowDialog();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"生成预览图时出错：{ex.Message}", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        /// <summary>
        /// 外层单线对地数据预览
        /// </summary>
        private void Tab2DataGrid_PreviewData(object sender, RoutedEventArgs e)
        {
            var selectedItem = Tab2DataGrid.SelectedItem as 外层单线对地_Model;
            if (selectedItem != null)
            {
                selectedItem.Calculate_Zo();
                //selectedItem.Zo 重新刷新到源中，更新UI显示的数据
                Tab2DataGrid.Items.Refresh();
                try
                {
                    var bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.UriSource = new Uri("pack://application:,,,/Assets/外层单线对地-si9000.jpg");
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.EndInit();

                    var drawingVisual = new DrawingVisual();
                    using (DrawingContext drawingContext = drawingVisual.RenderOpen())
                    {
                        drawingContext.DrawImage(bitmap, new Rect(0, 0, bitmap.PixelWidth, bitmap.PixelHeight));

                        var typeface = new Typeface(
                            new FontFamily("Segoe UI"),
                            FontStyles.Normal,
                            FontWeights.Normal,
                            FontStretches.Normal);
                        var brush = new SolidColorBrush(Colors.Black);

                        double w2 = ((selectedItem.W1 / 0.0254) - 1) * 0.0254;
                        var values = new List<string>
                        {
                            selectedItem.H1.ToString("F4"),
                            selectedItem.Er1.ToString("F4"),
                            selectedItem.W1.ToString("F4"),
                            w2.ToString("F4"),
                            selectedItem.D1.ToString("F4"),
                            selectedItem.T1.ToString("F4"),
                            selectedItem.C1.ToString("F4"),
                            selectedItem.C2.ToString("F4"),
                            selectedItem.CEr.ToString("F4"),
                            selectedItem.Zo.ToString("F2") ?? "0.00",
                        };

                        int[] yCoordinates = { 16, 40, 64, 88, 112, 136, 160, 184, 208, 255 };
                        int[] xCoordinate = { 578, 578, 578, 578, 578, 578, 578, 578, 578, 585 };
                        float fontSize = 13;

                        for (int i = 0; i < values.Count; i++)
                        {
                            if (!string.IsNullOrEmpty(values[i]))
                            {
                                var formattedText = new FormattedText(
                                    values[i],
                                    System.Globalization.CultureInfo.CurrentCulture,
                                    FlowDirection.LeftToRight,
                                    typeface,
                                    fontSize,
                                    brush,
                                    VisualTreeHelper.GetDpi(this).PixelsPerDip);

                                drawingContext.DrawText(formattedText, new Point(xCoordinate[i], yCoordinates[i]));
                            }
                        }
                    }

                    var renderBitmap = new RenderTargetBitmap(
                        bitmap.PixelWidth, bitmap.PixelHeight,
                        96, 96, PixelFormats.Pbgra32);
                    renderBitmap.Render(drawingVisual);

                    var previewWindow = new PreviewWindow(renderBitmap);
                    previewWindow.ShowDialog();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"生成预览图时出错：{ex.Message}", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        /// <summary>
        /// 内层单线对地数据预览
        /// </summary>
        private void Tab4DataGrid_PreviewData(object sender, RoutedEventArgs e)
        {
            var selectedItem = Tab4DataGrid.SelectedItem as 内层单线对地_Model;
            if (selectedItem != null)
            {
                selectedItem.Calculate_Zo();
                //selectedItem.Zo 重新刷新到源中，更新UI显示的数据
                Tab4DataGrid.Items.Refresh();
                try
                {
                    var bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.UriSource = new Uri("pack://application:,,,/Assets/内层单线对地-si9000.jpg");
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.EndInit();

                    var drawingVisual = new DrawingVisual();
                    using (DrawingContext drawingContext = drawingVisual.RenderOpen())
                    {
                        drawingContext.DrawImage(bitmap, new Rect(0, 0, bitmap.PixelWidth, bitmap.PixelHeight));

                        var typeface = new Typeface(
                            new FontFamily("Segoe UI"),
                            FontStyles.Normal,
                            FontWeights.Normal,
                            FontStretches.Normal);
                        var brush = new SolidColorBrush(Colors.Black);

                        double w2 = ((selectedItem.W1 / 0.0254) - 1) * 0.0254;
                        var values = new List<string>
                        {
                            selectedItem.H1.ToString("F4"),
                            selectedItem.Er1.ToString("F4"),
                            selectedItem.H2.ToString("F4"),
                            selectedItem.Er2.ToString("F4"),
                            selectedItem.W1.ToString("F4"),
                            w2.ToString("F4"),
                            selectedItem.D1.ToString("F4"),
                            selectedItem.T1.ToString("F4"),
                            selectedItem.Zo.ToString("F2") ?? "0.00",
                        };
                        int[] yCoordinates = { 16, 40, 64, 88, 112, 136, 160, 184, 232 };
                        int[] xCoordinate = { 578, 578, 578, 578, 578, 578, 578, 578, 585 };
                        float fontSize = 13;

                        for (int i = 0; i < values.Count; i++)
                        {
                            if (!string.IsNullOrEmpty(values[i]))
                            {
                                var formattedText = new FormattedText(
                                    values[i],
                                    System.Globalization.CultureInfo.CurrentCulture,
                                    FlowDirection.LeftToRight,
                                    typeface,
                                    fontSize,
                                    brush,
                                    VisualTreeHelper.GetDpi(this).PixelsPerDip);

                                drawingContext.DrawText(formattedText, new Point(xCoordinate[i], yCoordinates[i]));
                            }
                        }
                    }

                    var renderBitmap = new RenderTargetBitmap(
                        bitmap.PixelWidth, bitmap.PixelHeight,
                        96, 96, PixelFormats.Pbgra32);
                    renderBitmap.Render(drawingVisual);

                    var previewWindow = new PreviewWindow(renderBitmap);
                    previewWindow.ShowDialog();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"生成预览图时出错：{ex.Message}", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        /// <summary>
        /// 外层双线不对地数据预览
        /// </summary>
        private void Tab5DataGrid_PreviewData(object sender, RoutedEventArgs e)
        {
            var selectedItem = Tab5DataGrid.SelectedItem as 外层双线不对地_Model;
            if (selectedItem != null)
            {
                selectedItem.Calculate_Zo();
                //selectedItem.Zo 重新刷新到源中，更新UI显示的数据
                Tab5DataGrid.Items.Refresh();
                try
                {
                    var bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.UriSource = new Uri("pack://application:,,,/Assets/外层双线不对地-si9000.jpg");
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.EndInit();

                    var drawingVisual = new DrawingVisual();
                    using (DrawingContext drawingContext = drawingVisual.RenderOpen())
                    {
                        drawingContext.DrawImage(bitmap, new Rect(0, 0, bitmap.PixelWidth, bitmap.PixelHeight));

                        var typeface = new Typeface(
                            new FontFamily("Segoe UI"),
                            FontStyles.Normal,
                            FontWeights.Normal,
                            FontStretches.Normal);
                        var brush = new SolidColorBrush(Colors.Black);

                        double w2 = ((selectedItem.W1 / 0.0254) - 1) * 0.0254;
                        var values = new List<string>
                        {
                            selectedItem.H1.ToString("F4"),
                            selectedItem.Er1.ToString("F4"),
                            selectedItem.W1.ToString("F4"),
                            w2.ToString("F4"),
                            selectedItem.S1.ToString("F4"),
                            selectedItem.T1.ToString("F4"),
                            selectedItem.C1.ToString("F4"),
                            selectedItem.C2.ToString("F4"),
                            selectedItem.C3.ToString("F4"),
                            selectedItem.CEr.ToString("F4"),
                            selectedItem.Zo.ToString("F2") ?? "0.00"
                        };
                        int[] yCoordinates = { 16, 40, 64, 88, 112, 136, 160, 184, 208, 232, 279 };
                        int[] xCoordinate = { 578, 578, 578, 578, 578, 578, 578, 578, 578, 578, 585 };
                        float fontSize = 13;

                        for (int i = 0; i < values.Count; i++)
                        {
                            if (!string.IsNullOrEmpty(values[i]))
                            {
                                var formattedText = new FormattedText(
                                    values[i],
                                    System.Globalization.CultureInfo.CurrentCulture,
                                    FlowDirection.LeftToRight,
                                    typeface,
                                    fontSize,
                                    brush,
                                    VisualTreeHelper.GetDpi(this).PixelsPerDip);

                                drawingContext.DrawText(formattedText, new Point(xCoordinate[i], yCoordinates[i]));
                            }
                        }
                    }

                    var renderBitmap = new RenderTargetBitmap(
                        bitmap.PixelWidth, bitmap.PixelHeight,
                        96, 96, PixelFormats.Pbgra32);
                    renderBitmap.Render(drawingVisual);

                    var previewWindow = new PreviewWindow(renderBitmap);
                    previewWindow.ShowDialog();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"生成预览图时出错：{ex.Message}", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        /// <summary>
        /// 外层双线对地数据预览
        /// </summary>
        private void Tab6DataGrid_PreviewData(object sender, RoutedEventArgs e)
        {
            var selectedItem = Tab6DataGrid.SelectedItem as 外层双线对地_Model;
            if (selectedItem != null)
            {
                selectedItem.Calculate_Zo();
                //selectedItem.Zo 重新刷新到源中，更新UI显示的数据
                Tab6DataGrid.Items.Refresh();
                try
                {
                    var bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.UriSource = new Uri("pack://application:,,,/Assets/外层双线对地-si9000.jpg");
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.EndInit();

                    var drawingVisual = new DrawingVisual();
                    using (DrawingContext drawingContext = drawingVisual.RenderOpen())
                    {
                        drawingContext.DrawImage(bitmap, new Rect(0, 0, bitmap.PixelWidth, bitmap.PixelHeight));

                        var typeface = new Typeface(
                            new FontFamily("Segoe UI"),
                            FontStyles.Normal,
                            FontWeights.Normal,
                            FontStretches.Normal);
                        var brush = new SolidColorBrush(Colors.Black);

                        double w2 = ((selectedItem.W1 / 0.0254) - 1) * 0.0254;
                        var values = new List<string>
                        {
                            selectedItem.H1.ToString("F4"),
                            selectedItem.Er1.ToString("F4"),
                            selectedItem.W1.ToString("F4"),
                            w2.ToString("F4"),
                            selectedItem.S1.ToString("F4"),
                            selectedItem.D1.ToString("F4"),
                            selectedItem.T1.ToString("F4"),
                            selectedItem.C1.ToString("F4"),
                            selectedItem.C2.ToString("F4"),
                            selectedItem.C3.ToString("F4"),
                            selectedItem.CEr.ToString("F4"),
                            selectedItem.Zo.ToString("F2") ?? "0.00",
                        };
                        int[] yCoordinates = { 16, 40, 64, 88, 112, 136, 160, 184, 208, 232, 256, 303 };
                        int[] xCoordinate = { 578, 578, 578, 578, 578, 578, 578, 578, 578, 578, 578, 585 };
                        float fontSize = 13;

                        for (int i = 0; i < values.Count; i++)
                        {
                            if (!string.IsNullOrEmpty(values[i]))
                            {
                                var formattedText = new FormattedText(
                                    values[i],
                                    System.Globalization.CultureInfo.CurrentCulture,
                                    FlowDirection.LeftToRight,
                                    typeface,
                                    fontSize,
                                    brush,
                                    VisualTreeHelper.GetDpi(this).PixelsPerDip);

                                drawingContext.DrawText(formattedText, new Point(xCoordinate[i], yCoordinates[i]));
                            }
                        }
                    }

                    var renderBitmap = new RenderTargetBitmap(
                        bitmap.PixelWidth, bitmap.PixelHeight,
                        96, 96, PixelFormats.Pbgra32);
                    renderBitmap.Render(drawingVisual);

                    var previewWindow = new PreviewWindow(renderBitmap);
                    previewWindow.ShowDialog();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"生成预览图时出错：{ex.Message}", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        /// <summary>
        /// 内层双线不对地数据预览
        /// </summary>
        private void Tab7DataGrid_PreviewData(object sender, RoutedEventArgs e)
        {
            var selectedItem = Tab7DataGrid.SelectedItem as 内层双线不对地_Model;
            if (selectedItem != null)
            {
                selectedItem.Calculate_Zo();
                //selectedItem.Zo 重新刷新到源中，更新UI显示的数据
                Tab7DataGrid.Items.Refresh();
                try
                {
                    var bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.UriSource = new Uri("pack://application:,,,/Assets/内层双线不对地-si9000.jpg");
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.EndInit();

                    var drawingVisual = new DrawingVisual();
                    using (DrawingContext drawingContext = drawingVisual.RenderOpen())
                    {
                        drawingContext.DrawImage(bitmap, new Rect(0, 0, bitmap.PixelWidth, bitmap.PixelHeight));

                        var typeface = new Typeface(
                            new FontFamily("Segoe UI"),
                            FontStyles.Normal,
                            FontWeights.Normal,
                            FontStretches.Normal);
                        var brush = new SolidColorBrush(Colors.Black);

                        double w2 = ((selectedItem.W1 / 0.0254) - 1) * 0.0254;
                        var values = new List<string>
                        {
                            selectedItem.H1.ToString("F4"),
                            selectedItem.Er1.ToString("F4"),
                            selectedItem.H2.ToString("F4"),
                            selectedItem.Er2.ToString("F4"),
                            selectedItem.W1.ToString("F4"),
                            w2.ToString("F4"),
                            selectedItem.S1.ToString("F4"),
                            selectedItem.T1.ToString("F4"),
                            selectedItem.Zo.ToString("F2") ?? "0.00",
                        };

                        int[] yCoordinates = { 16, 40, 64, 88, 112, 136, 160, 184, 231 };
                        int[] xCoordinate = { 578, 578, 578, 578, 578, 578, 578, 578, 585 };
                        float fontSize = 13;

                        for (int i = 0; i < values.Count; i++)
                        {
                            if (!string.IsNullOrEmpty(values[i]))
                            {
                                var formattedText = new FormattedText(
                                    values[i],
                                    System.Globalization.CultureInfo.CurrentCulture,
                                    FlowDirection.LeftToRight,
                                    typeface,
                                    fontSize,
                                    brush,
                                    VisualTreeHelper.GetDpi(this).PixelsPerDip);

                                drawingContext.DrawText(formattedText, new Point(xCoordinate[i], yCoordinates[i]));
                            }
                        }
                    }

                    var renderBitmap = new RenderTargetBitmap(
                        bitmap.PixelWidth, bitmap.PixelHeight,
                        96, 96, PixelFormats.Pbgra32);
                    renderBitmap.Render(drawingVisual);

                    var previewWindow = new PreviewWindow(renderBitmap);
                    previewWindow.ShowDialog();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"生成预览图时出错：{ex.Message}", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        /// <summary>
        /// 内层双线对地数据预览
        /// </summary>
        private void Tab8DataGrid_PreviewData(object sender, RoutedEventArgs e)
        {
            var selectedItem = Tab8DataGrid.SelectedItem as 内层双线对地_Model;
            if (selectedItem != null)
            {
                selectedItem.Calculate_Zo();
                //selectedItem.Zo 重新刷新到源中，更新UI显示的数据
                Tab8DataGrid.Items.Refresh();
                try
                {
                    var bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.UriSource = new Uri("pack://application:,,,/Assets/内层双线对地-si9000.jpg");
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.EndInit();

                    var drawingVisual = new DrawingVisual();
                    using (DrawingContext drawingContext = drawingVisual.RenderOpen())
                    {
                        drawingContext.DrawImage(bitmap, new Rect(0, 0, bitmap.PixelWidth, bitmap.PixelHeight));

                        var typeface = new Typeface(
                            new FontFamily("Segoe UI"),
                            FontStyles.Normal,
                            FontWeights.Normal,
                            FontStretches.Normal);
                        var brush = new SolidColorBrush(Colors.Black);

                        double w2 = ((selectedItem.W1 / 0.0254) - 1) * 0.0254;
                        var values = new List<string>
                        {
                            selectedItem.H1.ToString("F4"),
                            selectedItem.Er1.ToString("F4"),
                            selectedItem.H2.ToString("F4"),
                            selectedItem.Er2.ToString("F4"),
                            selectedItem.W1.ToString("F4"),
                            w2.ToString("F4"),
                            selectedItem.S1.ToString("F4"),
                            selectedItem.D1.ToString("F4"),
                            selectedItem.T1.ToString("F4"),
                            selectedItem.Zo.ToString("F2") ?? "0.00",
                        };
                        int[] yCoordinates = { 16, 40, 64, 88, 112, 136, 160, 184, 208, 256 };
                        int[] xCoordinate = { 578, 578, 578, 578, 578, 578, 578, 578, 578, 585 };
                        float fontSize = 13;

                        for (int i = 0; i < values.Count; i++)
                        {
                            if (!string.IsNullOrEmpty(values[i]))
                            {
                                var formattedText = new FormattedText(
                                    values[i],
                                    System.Globalization.CultureInfo.CurrentCulture,
                                    FlowDirection.LeftToRight,
                                    typeface,
                                    fontSize,
                                    brush,
                                    VisualTreeHelper.GetDpi(this).PixelsPerDip);

                                drawingContext.DrawText(formattedText, new Point(xCoordinate[i], yCoordinates[i]));
                            }
                        }
                    }

                    var renderBitmap = new RenderTargetBitmap(
                        bitmap.PixelWidth, bitmap.PixelHeight,
                        96, 96, PixelFormats.Pbgra32);
                    renderBitmap.Render(drawingVisual);

                    var previewWindow = new PreviewWindow(renderBitmap);
                    previewWindow.ShowDialog();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"生成预览图时出错：{ex.Message}", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        /// <summary>
        /// 内层单线不对地数据预览
        /// </summary>
        private void Tab3DataGrid_PreviewData(object sender, RoutedEventArgs e)
        {
            var selectedItem = Tab3DataGrid.SelectedItem as 内层单线不对地_Model;
            if (selectedItem != null)
            {
                selectedItem.Calculate_Zo();
                //selectedItem.Zo 重新刷新到源中，更新UI显示的数据
                Tab3DataGrid.Items.Refresh();
                try
                {
                    var bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.UriSource = new Uri("pack://application:,,,/Assets/内层单线不对地-Si9000.jpg");
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.EndInit();

                    var drawingVisual = new DrawingVisual();
                    using (DrawingContext drawingContext = drawingVisual.RenderOpen())
                    {
                        drawingContext.DrawImage(bitmap, new Rect(0, 0, bitmap.PixelWidth, bitmap.PixelHeight));

                        var typeface = new Typeface(
                            new FontFamily("Segoe UI"),
                            FontStyles.Normal,
                            FontWeights.Normal,
                            FontStretches.Normal);
                        var brush = new SolidColorBrush(Colors.Black);

                        double w2 = ((selectedItem.W1 / 0.0254) - 1) * 0.0254;
                        var values = new List<string>
                        {
                            selectedItem.H1.ToString("F4"),
                            selectedItem.Er1.ToString("F4"),
                            selectedItem.H2.ToString("F4"),
                            selectedItem.Er2.ToString("F4"),
                            selectedItem.W1.ToString("F4"),
                            w2.ToString("F4"),
                            selectedItem.T1.ToString("F4"),
                            selectedItem.Zo.ToString("F2") ?? "0.00",
                        };

                        int[] yCoordinates = { 16, 40, 64, 88, 112, 136, 160, 208 };
                        int[] xCoordinate = { 578, 578, 578, 578, 578, 578, 578, 585 };

                        float fontSize = 13;

                        for (int i = 0; i < values.Count; i++)
                        {
                            if (!string.IsNullOrEmpty(values[i]))
                            {
                                var formattedText = new FormattedText(
                                    values[i],
                                    System.Globalization.CultureInfo.CurrentCulture,
                                    FlowDirection.LeftToRight,
                                    typeface,
                                    fontSize,
                                    brush,
                                    VisualTreeHelper.GetDpi(this).PixelsPerDip);

                                drawingContext.DrawText(formattedText, new Point(xCoordinate[i], yCoordinates[i]));
                            }
                        }
                    }

                    var renderBitmap = new RenderTargetBitmap(
                        bitmap.PixelWidth, bitmap.PixelHeight,
                        96, 96, PixelFormats.Pbgra32);
                    renderBitmap.Render(drawingVisual);

                    var previewWindow = new PreviewWindow(renderBitmap);
                    previewWindow.ShowDialog();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"生成预览图时出错：{ex.Message}", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}