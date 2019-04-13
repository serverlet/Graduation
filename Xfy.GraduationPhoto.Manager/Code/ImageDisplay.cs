using System.ComponentModel;
using System.Windows;
using System.Windows.Media;

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

        public Stretch _stretch;

        public Stretch Stretch
        {
            get => _stretch;
            set
            {
                if (_stretch == value)
                {
                    return;
                }
                _stretch = value;
                this.PropertyChanged(this, new PropertyChangedEventArgs(nameof(Stretch)));
            }
        }

        private double _width;

        public double Width
        {
            get => _width;
            set
            {
                if (_width == value)
                {
                    return;
                }
                _width = value;
                this.PropertyChanged(this, new PropertyChangedEventArgs(nameof(Width)));
            }
        }

        private double _height;
        public double Height
        {
            get => _height;
            set
            {
                if (_height == value)
                {
                    return;
                }
                _height = value;
                this.PropertyChanged(this, new PropertyChangedEventArgs(nameof(Height)));
            }
        }

        private System.Windows.Media.ScaleTransform _RenderTransform;

        public System.Windows.Media.ScaleTransform RenderTransform
        {
            get => this._RenderTransform;
            set { _RenderTransform = value; this.PropertyChanged(this, new PropertyChangedEventArgs(nameof(RenderTransform))); }
        }

        private Point _RenderTransformOrigin;

        public Point RenderTransformOrigin
        {
            get => this._RenderTransformOrigin;
            set { _RenderTransformOrigin = value; this.PropertyChanged(this, new PropertyChangedEventArgs(nameof(RenderTransformOrigin))); }
        }


        public int Index { get; internal set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public ImageDisplay()
        {
            //RenderTransformOrigin = new Point(0.5, 0.5);
            //RenderTransform = new ScaleTransform(1, 1);
        }
    }
}
