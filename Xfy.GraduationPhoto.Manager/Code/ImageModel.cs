﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xfy.GraduationPhoto.Manager.Code
{
    public class ImageModel
    {

        public DateTime? PhotoDate { get; set; }

        /// <summary>
        /// 经度
        /// </summary>
        public double? Longitude { get; set; }

        /// <summary>
        /// 维度
        /// </summary>
        public double? Latitude { get; set; }

        /// <summary>
        /// 高德地图格式化之后的地址
        /// </summary>
        public string Address { get; internal set; }
    }
}
