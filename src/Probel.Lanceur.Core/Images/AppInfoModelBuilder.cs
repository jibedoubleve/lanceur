﻿using HistoryManager.Apps;
using HistoryViewer.Infrastructure.Image;
using HistoryViewer.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace HistoryViewer.Infrastructure
{
    internal static class AppInfoModelBuilder
    {
        #region Fields

        private static readonly string[] ImageExtensions =
        {
            ".png",
            ".jpg",
            ".jpeg",
            ".gif",
            ".bmp",
            ".tiff",
            ".ico"
        };

        private static ImageCache ImageCache = ImageCache.Instance;

        #endregion Fields

        #region Enums

        private enum ImageType
        {
            File,
            Folder,
            Data,
            ImageFile,
            Error,
            Cache
        }

        #endregion Enums

        #region Methods

        private static ImageSource GetImage(string path)
        {
            var img = LoadImage(path);
            ImageCache[path] = img;
            return img;
        }

        private static ImageSource LoadImage(string path)
        {
            ImageSource image = null;
            ImageType type = ImageType.Error;
            try
            {
                if (string.IsNullOrEmpty(path))
                {
                    //    return new ImageResult(ImageCache[Constant.ErrorIcon], ImageType.Error);
                }
                if (ImageCache.ContainsKey(path))
                {
                    return ImageCache[path];
                }

                if (path?.StartsWith("data:", StringComparison.OrdinalIgnoreCase) ?? false)
                {
                    var imageSource = new BitmapImage(new Uri(path));
                    imageSource.Freeze();
                    return imageSource;
                }

                if (!Path.IsPathRooted(path))
                {
                    path = Path.Combine(Constant.ProgramDirectory, "Images", Path.GetFileName(path));
                }

                if (Directory.Exists(path))
                {
                    /* Directories can also have thumbnails instead of shell icons.
                     * Generating thumbnails for a bunch of folders while scrolling through
                     * results from Everything makes a big impact on performance and
                     * Wox responsibility.
                     * - Solution: just load the icon
                     */
                    type = ImageType.Folder;
                    image = WindowsThumbnailProvider.GetThumbnail(path, Constant.ThumbnailSize,
                        Constant.ThumbnailSize, ThumbnailOptions.IconOnly);
                }
                else if (File.Exists(path))
                {
                    var extension = Path.GetExtension(path).ToLower();
                    if (ImageExtensions.Contains(extension))
                    {
                        type = ImageType.ImageFile;
                        /* Although the documentation for GetImage on MSDN indicates that
                         * if a thumbnail is available it will return one, this has proved to not
                         * be the case in many situations while testing.
                         * - Solution: explicitly pass the ThumbnailOnly flag
                         */
                        image = WindowsThumbnailProvider.GetThumbnail(path, Constant.ThumbnailSize,
                            Constant.ThumbnailSize, ThumbnailOptions.ThumbnailOnly);
                    }
                    else
                    {
                        type = ImageType.File;
                        image = WindowsThumbnailProvider.GetThumbnail(path, Constant.ThumbnailSize,
                            Constant.ThumbnailSize, ThumbnailOptions.None);
                    }
                }
                //else
                //{
                //    image = ImageCache[Constant.ErrorIcon];
                //    path = Constant.ErrorIcon;
                //}

                if (type != ImageType.Error)
                {
                    image.Freeze();
                }
            }
            catch (System.Exception e)
            {
                Trace.WriteLine($"|ImageLoader.Load|Failed to get thumbnail for {path} - {e}");
                type = ImageType.Error;
                //image = ImageCache[Constant.ErrorIcon];
                //ImageCache[path] = image;
            }
            return image;
        }

        public static void Initialize()
        {
            Task.Run(() =>
            {
                ImageCache.GetKeys().AsParallel().ForAll(x =>
                {
                    GetImage(x);
                });
            });
        }

        public static IEnumerable<AppInfoModel> ToModel(this IEnumerable<AppInfo> src)
        {
            var dst = new List<AppInfoModel>();
            foreach (var item in src)
            {
                if (!item.IsEmpty)
                {
                    var d = new AppInfoModel(
                        path: item.Path,
                        name: item.Name,
                        image: GetImage(item.Path)
                    );
                    dst.Add(d);
                }
            }

            return dst;
        }

        #endregion Methods
    }
}