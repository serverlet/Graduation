using System;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace Xfy.GraduationPhoto.Manager
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        #region 只读字段 
        /// <summary>
        /// 
        /// </summary>
        public readonly Code.LoadImagePath LoadImagePath;

        /// <summary>
        /// 图片路径队列
        /// </summary>
        public readonly ConcurrentQueue<FileInfo> ImagePathQueue;
        #endregion

        #region 属性
        /// <summary>
        /// 
        /// </summary>
        public Code.StatusContent StatusContent { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Code.ImageDisplay ImageDisplay { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public FileInfo[] ImagePaths { get; set; }
        #endregion

        /// <summary>
        /// 构造函数
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += MainWindow_Loaded;
            StatusContent = new Code.StatusContent();
            LoadImagePath = new Code.LoadImagePath();
            ImageDisplay = new Code.ImageDisplay();
            ImagePathQueue = new ConcurrentQueue<FileInfo>();

            this.StatusBar_Status.DataContext = StatusContent;
            this.Sp_MainContainer.DataContext = ImageDisplay;
        }

        /// <summary>
        /// 窗口加载完成
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            this.MeunItem_OpenFolder.Click += MeunItem_OpenFolder_Click;
            this.MeunItem_Arrange.Click += MeunItem_Arrange_Click;

            this.LoadImagePath.ReadCompletedHanander += LoadImagePath_ReadCompletedHanander;
            this.LoadImagePath.ReadHanander += LoadImagePath_ReadHanander;
            this.Img_Prev.MouseUp += Img_MouseUp;
            this.Img_Next.MouseUp += Img_MouseUp;
        }

        /// <summary>
        /// 读取中产生的事件
        /// </summary>
        /// <param name="arg1"></param>
        /// <param name="arg2"></param>
        private void LoadImagePath_ReadHanander(object arg1, Code.ImagePathEventArgs arg2)
        {
            foreach (FileInfo item in arg2.ImagePaths)
            {
                ImagePathQueue.Enqueue(item);//进入到队列
            }
        }

        /// <summary>
        /// 开始整理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MeunItem_Arrange_Click(object sender, RoutedEventArgs e)
        {
            ParallelLoopResult state = Parallel.For(0, 10, _ =>
            {
                while (ImagePathQueue.TryDequeue(out FileInfo fileInfo))
                {
                    //TODO...
                }
            });//10个线程跑
        }

        /// <summary>
        /// 左箭头，右箭头的click事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Img_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            FrameworkElement uIElement;
            FileInfo imageFile;
            if (e.LeftButton == System.Windows.Input.MouseButtonState.Released)
            {
                if (ImagePaths == null || ImagePaths.Length == 0)
                {
                    return;
                }
                uIElement = sender as FrameworkElement;
                int direction = Convert.ToInt32(uIElement.Tag);
                //处理
                int index = ImageDisplay.Index;
                index += direction * 1;
                if (index == ImagePaths.Length)
                {
                    index = 0;
                }
                if (index < 0)
                {
                    index = ImagePaths.Length - 1;
                }
                ImageDisplay.Index = index;
                imageFile = ImagePaths[index];
                HanadleImage(imageFile);
            }
        }

        /// <summary>
        /// 图片完成事件
        /// </summary>
        /// <param name="arg1"></param>
        /// <param name="arg2"></param>
        private void LoadImagePath_ReadCompletedHanander(object arg1, Code.ImagePathEventArgs arg2)
        {
            FileInfo imageFile;
            if (arg2.ImagePaths.Any())
            {
                imageFile = arg2.ImagePaths.FirstOrDefault();
                StatusContent.ImageCount = arg2.ImagePaths.Count();
                ImageDisplay.ImagePath = imageFile.FullName;
                ImageDisplay.Index = 0;
                ImagePaths = arg2.ImagePaths.ToArray();

                HanadleImage(imageFile);
                //StatusContent.OwnerPath = ImagePaths[index].DirectoryName;
            }
        }

        /// <summary>
        /// 打开文件夹
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void MeunItem_OpenFolder_Click(object sender, RoutedEventArgs e)
        {
            using (System.Windows.Forms.FolderBrowserDialog dialog = new System.Windows.Forms.FolderBrowserDialog())
            {
                System.Windows.Forms.DialogResult result = dialog.ShowDialog();
                if (result != System.Windows.Forms.DialogResult.OK)
                {
                    return;
                }
                StatusContent.CurrentFolder = dialog.SelectedPath;
                await LoadImagePath.Get(dialog.SelectedPath);
                //这里要判断是否是同一个文件加，不是清空队列
            }
        }

        public void HanadleImage(FileInfo imageFile)
        {
            BitmapImage bitmapImage = new BitmapImage(new Uri(imageFile.FullName));
            if (bitmapImage.PixelWidth > Sp_MainContainer.ActualWidth || bitmapImage.PixelHeight > Sp_MainContainer.ActualHeight)
            {
                //缩放
                ImageDisplay.Stretch = System.Windows.Media.Stretch.UniformToFill;
                ImageDisplay.Height = bitmapImage.PixelHeight;
                ImageDisplay.Width = bitmapImage.PixelWidth;
            }
            else
            {
                ImageDisplay.Stretch = System.Windows.Media.Stretch.None;
            }
            ImageDisplay.ImagePath = imageFile.FullName;
            StatusContent.OwnerPath = imageFile.DirectoryName;
        }
    }
}
