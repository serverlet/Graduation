using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xfy.GraduationPhoto.Manager.Code.Model
{
    [Table("ImageTable")]
    public class ImageEntity
    {
        [Key]
        public string Id { get; set; }

        public double Lat { get; set; }

        public double Lng { get; set; }

        public string FileName { get; set; }

        public string Md5Hash { get; set; }

        public string Path { get; set; }
    }
}
