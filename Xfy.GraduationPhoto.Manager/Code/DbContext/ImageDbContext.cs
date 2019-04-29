using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xfy.GraduationPhoto.Manager.Code.Model;

namespace Xfy.GraduationPhoto.Manager.Code.DbContext
{
    public class ImageDbContext : System.Data.Entity.DbContext
    {
        public ImageDbContext() : base("con")
        {

        }

        public DbSet<ImageEntity> ImageModels { get; set; }
    }
}
