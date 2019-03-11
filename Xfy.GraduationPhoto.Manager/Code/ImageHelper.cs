using MetadataExtractor;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Rational = MetadataExtractor.Rational;

namespace Xfy.GraduationPhoto.Manager.Code
{
    public class ImageHelper
    {
        //private class
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        /// <summary>
        /// 获取图片的拍摄事件经纬度（如果有）
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static ImageModel HanadleImage(FileInfo file)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            ImageModel model = new ImageModel();
            //using (Image<Rgba32> image = Image.Load(file.FullName))
            //{
            #region 原始版本 .net framework下慢 .net core下快
            //ExifValue photoDateValue = image?.MetaData?.ExifProfile?.GetValue(ExifTag.DateTimeOriginal);

            //if (photoDateValue == null)
            //{
            //    model.PhotoDate = file.CreationTime;
            //    return model;
            //}
            //CultureInfo enUS = new CultureInfo("en-US");
            //bool isConvertSucced = DateTime.TryParseExact(photoDateValue?.Value?.ToString(), "yyyy:MM:dd HH:mm:ss", enUS, DateTimeStyles.None, out DateTime result);//YYYY:MM:DD HH:mm:ss
            //if (isConvertSucced)
            //{
            //    model.PhotoDate = result;
            //}
            //else
            //{
            //    model.PhotoDate = file.CreationTime;
            //}

            //ExifValue latitudeValue = image.MetaData.ExifProfile.GetValue(ExifTag.GPSLatitude);
            //if (latitudeValue != null && latitudeValue.Value != null)
            //{
            //    Rational[] latitudeArray = latitudeValue.Value as Rational[];
            //    if (latitudeArray != null)
            //    {
            //        model.Latitude = latitudeArray.ToDouble();
            //    }
            //}

            //ExifValue longitudeValue = image.MetaData.ExifProfile.GetValue(ExifTag.GPSLongitude);
            //if (longitudeValue != null && longitudeValue.Value != null)
            //{
            //    Rational[] longitudeValueArray = longitudeValue.Value as Rational[];
            //    if (longitudeValueArray != null)
            //    {
            //        model.Longitude = longitudeValueArray.ToDouble();
            //    }
            //} 
            #endregion
            IEnumerable<MetadataExtractor.Directory> directories = ImageMetadataReader.ReadMetadata(file.FullName).Where(_ => _.Name == "GPS" || _.Name == "Exif SubIFD");
            if (!directories.Any())
            {
                model.PhotoDate = file.CreationTime;
                return model;
            }
            foreach (MetadataExtractor.Directory item in directories)
            {
                switch (item.Name)
                {
                    case "Exif SubIFD":
                        if (item.TryGetDateTime(36867, out DateTime create))
                        {
                            model.PhotoDate = create;
                        }
                        break;
                    case "GPS":
                        Rational[] la = item.GetRationalArray(2);//纬度
                                                                 //Console.WriteLine();
                        model.Latitude = la.ToDouble();
                        model.Longitude = item.GetRationalArray(4).ToDouble();
                        break;
                    default:
                        break;
                }
            }
            //}
            sw.Stop();
            logger.Debug($"当前线程：{System.Threading.Thread.CurrentThread.ManagedThreadId}\n文件名：{file.FullName}\n耗时：{sw.Elapsed}\n文件大小：{Math.Round(file.Length * 1.0 / 1024 / 1024, 0)}MB\n*******************************************************************\n");
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
            if (value == null)
            {
                return 0;
            }
            return value[0].Numerator * 1.0 / value[0].Denominator + value[1].Numerator * 1.0 / value[1].Denominator / 60 + value[2].Numerator * 1.0 / value[2].Denominator / 3600;
        }
    }
}
