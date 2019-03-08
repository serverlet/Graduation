using System.Collections.Generic;
using System.IO;
using System.Security.AccessControl;
using System.Linq;
using System.Text.RegularExpressions;
using System;
using System.Threading.Tasks;

namespace Xfy.GraduationPhoto.Manager.Code
{
    /// <summary>
    /// 获取指定文件夹下面的所有图片
    /// </summary>
    public class LoadImagePath
    {
        //private readonly string SearchPattern;

        private readonly Regex ImageRegex;

        /// <summary>
        /// 读取中
        /// </summary>
        public event Action<object, ImagePathEventArgs> ReadHanander;

        /// <summary>
        /// 读取结束
        /// </summary>
        public event Action<object, ImagePathEventArgs> ReadCompletedHanander;


        /// <summary>
        /// 读取开始
        /// </summary>
        public event Action<object, EventArgs> ReadStartHanander;

        public LoadImagePath()
        {
            string SearchPattern = System.Configuration.ConfigurationManager.AppSettings["SAFEEXTENSION"]?.Trim();
            if (string.IsNullOrEmpty(SearchPattern))
            {
                throw new System.Exception("读取配置文件出错，缺少键“{SAFEEXTENSION}”");
            }
            ImageRegex = new Regex(SearchPattern);
        }

        public async Task<IEnumerable<FileInfo>> Get(string folderPtah)
        {
            List<FileInfo> list = new List<FileInfo>();
            ReadStartHanander?.Invoke(this, EventArgs.Empty);
            GetImagePathRecursive(list, folderPtah);
            ReadCompletedHanander?.Invoke(this, new ImagePathEventArgs() { ImagePaths = list });
            return await Task.FromResult(list);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private void GetImagePathRecursive(List<FileInfo> list, string folderPtah)
        {
            DirectoryInfo currentDirectory;
            DirectoryInfo[] directories;
            IEnumerable<FileInfo> files;
            DirectorySecurity directorySecurity = new DirectorySecurity(folderPtah, AccessControlSections.Access);
            if (directorySecurity.AreAuditRulesProtected)
            {
                return;
            }
            currentDirectory = new DirectoryInfo(folderPtah);

            files = currentDirectory.GetFiles()
                .Where(_ => ImageRegex.IsMatch(_.Name.ToLower()));
                //.Select(_ => _.FullName);
            ReadHanander?.Invoke(this, new ImagePathEventArgs() { ImagePaths = files });
            list.AddRange(files);
            directories = currentDirectory.GetDirectories();
            foreach (DirectoryInfo item in directories)
            {
                GetImagePathRecursive(list, item.FullName);
            }
        }

    }

    public class ImagePathEventArgs: EventArgs
    {
        public IEnumerable<FileInfo> ImagePaths { get; set; }
    }
}
