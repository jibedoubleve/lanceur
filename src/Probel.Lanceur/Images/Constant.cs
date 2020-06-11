using System;
using System.IO;
using System.Reflection;

namespace Probel.Lanceur.Images
{
    internal class Constant
    {
        #region Fields

        private static readonly Assembly Assembly;

        public static readonly string ErrorIcon;

        public static readonly string ProgramDirectory;

        public static readonly int ThumbnailSize = 64;

        #endregion Fields

        #region Constructors

        static Constant()
        {
            Assembly = Assembly.GetExecutingAssembly();
            ProgramDirectory = Directory.GetParent(Assembly?.Location ?? throw new NullReferenceException()).ToString();
            ErrorIcon = Path.Combine(ProgramDirectory, "Assets", "app_error.png");
        }

        #endregion Constructors
    }

    public static class Helper
    {
        #region Methods

        /// <summary>
        /// http://www.yinwang.org/blog-cn/2015/11/21/programming-philosophy
        /// </summary>
        public static T NonNull<T>(this T obj)
        {
            if (obj == null)
            {
                throw new NullReferenceException();
            }
            else
            {
                return obj;
            }
        }

        #endregion Methods
    }
}