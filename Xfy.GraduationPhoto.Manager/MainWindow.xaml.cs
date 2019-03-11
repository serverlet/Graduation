using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using Xfy.GraduationPhoto.Manager.Code;

namespace Xfy.GraduationPhoto.Manager
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public static object Locker = new object();

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
            //int maxTask = (ImagePathQueue.Count % 5) == 0 ?;
            List<Task> tasks = new List<Task>();
            for (int i = 0; i < 5; i++)
            {
                Task task = Task.Factory.StartNew(executePhoto, ImagePathQueue);
                tasks.Add(task);
            }

            Task.Factory.StartNew(async _ =>
            {
                StatusContent.Status = "整理图片中...";
                Task t = _ as Task;
                await t;
                StatusContent.Status = "图片整理完成";
                MessageBox.Show("分类完成！即将打开整理后图片所在文件夹", "系统提示");
                System.Diagnostics.Process.Start("Explorer.exe", StatusContent.CurrentFolder);
            }, Task.WhenAll(tasks));

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
                SetImageStatus(imageFile);
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

                StatusContent.HandCount = arg2.ImagePaths.Count();
                SetImageStatus(imageFile);
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
                if (StatusContent.CurrentFolder == dialog.SelectedPath)
                {
                    return;
                }
                while (ImagePathQueue.TryDequeue(out FileInfo fileInfo))
                {
                    //清空队列
                }
                StatusContent.CurrentFolder = dialog.SelectedPath;
                await LoadImagePath.Get(dialog.SelectedPath);
                //这里要判断是否是同一个文件加，不是清空队列

            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="imageFile"></param>
        private void SetImageStatus(FileInfo imageFile)
        {
            BitmapImage bitmapImage = new BitmapImage(new Uri(imageFile.FullName));
            if (bitmapImage.PixelWidth > Sp_MainContainer.ActualWidth || bitmapImage.PixelHeight > Sp_MainContainer.ActualHeight)
            {
                //缩放
                ImageDisplay.Stretch = System.Windows.Media.Stretch.Uniform;
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

        private void executePhoto(object _)
        {
            ConcurrentQueue<FileInfo> qu = _ as ConcurrentQueue<FileInfo>;
            while (qu.TryDequeue(out FileInfo fileInfo))
            {
                //TODO...
                ImageModel result = ImageHelper.HanadleImage(fileInfo);
                string photoDateStr = result.PhotoDate.Value.ToString("yyyy年MM月dd号");
                DirectoryInfo directoryInfo = new DirectoryInfo($"{StatusContent.CurrentFolder}\\{photoDateStr}");
                lock (Locker)
                {
                    if (!directoryInfo.Exists)
                    {
                        directoryInfo.Create();
                    }
                    fileInfo.CopyTo($"{directoryInfo.FullName}\\{fileInfo.Name}", true);
                }
                StatusContent.HandCount = ImagePathQueue.Count;
            }
        }
    }
}
