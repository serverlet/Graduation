using System.ComponentModel;
using System.Windows;

namespace Xfy.GraduationPhoto.Manager
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private string _currentPath;

        public event PropertyChangedEventHandler PropertyChanged;

        public string CurrentPath
        {
            get => _currentPath;
            set
            {
                _currentPath = value;
                if (PropertyChanged != null && !string.IsNullOrEmpty(value) && (value != _currentPath || !string.IsNullOrEmpty(_currentPath)))
                {
                    this.PropertyChanged(this, new PropertyChangedEventArgs("CurrentPath"));
                };
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += MainWindow_Loaded;
            this.StatusBar_Status.DataContext = this;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            this.MeunItem_OpenFolder.Click += MeunItem_OpenFolder_Click;
        }

        /// <summary>
        /// 打开文件夹
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MeunItem_OpenFolder_Click(object sender, RoutedEventArgs e)
        {
            using (System.Windows.Forms.FolderBrowserDialog dialog = new System.Windows.Forms.FolderBrowserDialog())
            {
                System.Windows.Forms.DialogResult result = dialog.ShowDialog();
                if (result != System.Windows.Forms.DialogResult.OK)
                {
                    return;
                }
                CurrentPath = $"当前文件夹：{dialog.SelectedPath}";
            }
        }
    }
}
