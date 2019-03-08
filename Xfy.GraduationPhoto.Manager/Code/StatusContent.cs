using System.ComponentModel;
using System.Windows;

namespace Xfy.GraduationPhoto.Manager.Code
{
    /// <summary>
    /// 状态来内容
    /// </summary>
    public class StatusContent : INotifyPropertyChanged
    {
        private string _currentFolder;

        private int _imageCount;

        public string CurrentFolder
        {
            get => _currentFolder;
            set
            {
                _currentFolder = value;
                if (PropertyChanged != null && !string.IsNullOrEmpty(value) && (value != _currentFolder || !string.IsNullOrEmpty(_currentFolder)))
                {
                    this.PropertyChanged(this, new PropertyChangedEventArgs(nameof(CurrentFolder)));
                };
            }
        }

        public int ImageCount
        {
            get => _imageCount; set
            {
                _imageCount = value;
                if (PropertyChanged != null)
                {
                    this.PropertyChanged(this, new PropertyChangedEventArgs(nameof(ImageCount)));
                    this.PropertyChanged(this, new PropertyChangedEventArgs(nameof(CountVisibility)));
                };
            }
        }

        private string _ownerPath;

        public string OwnerPath
        {
            get => _ownerPath;
            set
            {
                _ownerPath = value;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged(this, new PropertyChangedEventArgs(nameof(OwnerPath)));
                }
            }
        }

        private int _handCount;

        public Visibility CountVisibility => _imageCount > 0 ? Visibility.Visible : Visibility.Hidden;

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
