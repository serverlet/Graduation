using SixLabors.ImageSharp;
using SixLabors.ImageSharp.MetaData.Profiles.Exif;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Primitives;
using System;
using System.IO;

namespace Xfy.GraduationPhoto.Manager.Code
{
    public class ImageHelper
    {
        /// <summary>
        /// 获取图片的拍摄事件经纬度（如果有）
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static ImageModel HanadleImage(FileInfo file)
        {
            ImageModel model = new ImageModel();
            using (Image<Rgba32> image = Image.Load(file.FullName))
            {
                ExifValue photoDateValue = image?.MetaData?.ExifProfile?.GetValue(ExifTag.DateTimeOriginal);
                if (photoDateValue == null)
                {
                    model.PhotoDate = file.CreationTime;
                    return model;
                }
                bool isConvertSucced = DateTime.TryParse(photoDateValue?.Value?.ToString(), out DateTime result);//YYYY:MM:DD HH:mm:ss
                if (isConvertSucced)
                {
                    model.PhotoDate = result;
                }
                else
                {
                    model.PhotoDate = file.CreationTime;
                }

                ExifValue latitudeValue = image.MetaData.ExifProfile.GetValue(ExifTag.GPSLatitude);
                if (latitudeValue != null && latitudeValue.Value != null)
                {
                    Rational[] latitudeArray = latitudeValue.Value as Rational[];
                    if (latitudeArray != null)
                    {
                        model.Latitude = latitudeArray.ToDouble();
                    }
                }

                ExifValue longitudeValue = image.MetaData.ExifProfile.GetValue(ExifTag.GPSLongitude);
                if (longitudeValue != null && longitudeValue.Value != null)
                {
                    Rational[] longitudeValueArray = longitudeValue.Value as Rational[];
                    if (longitudeValueArray != null)
                    {
                        model.Longitude = longitudeValueArray.ToDouble();
                    }
                }
            }
            return model;
        }
    }

    public static class Extension
    {
        /// <summary>
        /// 度° 分′ 秒″
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static double ToDouble(this Rational[] value)
        {
            return value[0].Numerator * 1.0 / value[0].Denominator + value[1].Numerator * 1.0 / value[1].Denominator / 60 + value[2].Numerator * 1.0 / value[2].Denominator / 3600;
        }
    }
}
