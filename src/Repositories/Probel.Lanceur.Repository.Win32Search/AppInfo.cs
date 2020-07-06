using System;
using System.Collections.Generic;
using System.Text;

namespace Probel.Lanceur.Repository.Win32Search
{
    public class AppInfo
    {
        #region Properties

        public static AppInfo Empty => new AppInfo { Name = null, Path = null };

        public bool IsEmpty => string.IsNullOrEmpty(Name) && string.IsNullOrEmpty(Path);
        public string Name { get; set; }

        public string Path { get; set; }

        #endregion Properties

        #region Methods

        public static AppInfo FromPath(string fileName)
        {
            if (string.IsNullOrEmpty(fileName)) { return AppInfo.Empty; }
            else
            {
                var fi = new System.IO.FileInfo(fileName);
                var name = fi.Name.Replace(fi.Extension, "");

                return new AppInfo
                {
                    Path = fileName,
                    Name = name,
                };
            }
        }

        #endregion Methods
    }
}
