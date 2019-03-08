using System.ComponentModel;

namespace Xfy.GraduationPhoto.Manager.Code
{
    public class ImageDisplay : INotifyPropertyChanged
    {
        private string _imagePath;

        public string ImagePath
        {
            get => _imagePath;
            set
            {
                if (string.IsNullOrEmpty(value.Trim()))
                {
                    return;
                }
                _imagePath = value.Trim();
                this.PropertyChanged(this, new PropertyChangedEventArgs(nameof(ImagePath)));
            }
        }

        public int Index { get; internal set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
