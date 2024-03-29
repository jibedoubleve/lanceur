﻿using Probel.Lanceur.Core.Entities;
using Probel.Lanceur.Models;
using Probel.Lanceur.SharedKernel.Logs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Probel.Lanceur.Images
{
    internal static class ResultItemHelper
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

        private static readonly ImageCache Cache = new ImageCache();

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

        #region Properties

        public static ILogService Logger { get; set; }

        #endregion Properties

        #region Methods

        private static ImageSource GetImage(AliasTextModel c, Query item)
        {
            if (c.IsPackaged)
            {
                if (string.IsNullOrEmpty(c.Icon)) { return GetImage(item.FileName); }
                else { return GetImage(item.Icon); }
            }
            else { return GetImage(item.FileName); }
        }

        private static ImageSource GetImage(string path)
        {
            if (Cache.ContainsKey(path)) { return Cache[path]; }
            else
            {
                var img = LoadImage(path);
                Cache[path] = img;
                return img;
            }
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
                    return null;
                }
                if (Cache.ContainsKey(path))
                {
                    return Cache[path];
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
            catch (Exception e)
            {
                var msg = $"|ImageLoader.Load|Failed to get thumbnail for {path} - {e}";
                Logger?.Warning(msg);

                //image = ImageCache[Constant.ErrorIcon];
                //ImageCache[path] = image;
            }
            return image;
        }

        public static IEnumerable<AliasTextModel> GetRefreshed(this IEnumerable<Query> src)
        {
            var dst = new List<AliasTextModel>();
            foreach (var item in src)
            {
                var c = new AliasTextModel(item);
                Task.Run(async () =>
                {
                    //await Task.Delay(10);
                    c.Image = GetImage(c, item);
                });
                dst.Add(c);
            }

            return dst;
        }

        public static void Initialize()
        {
            Task.Run(() =>
            {
                Cache.GetKeys().AsParallel().ForAll(x =>
                {
                    GetImage(x);
                });
            });
        }

        #endregion Methods
    }
}