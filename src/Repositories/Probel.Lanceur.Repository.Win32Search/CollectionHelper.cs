using Probel.Lanceur.Infrastructure.Extensions;
using Probel.Lanceur.Repositories;
using System.Collections.Generic;
using System.IO;

namespace Probel.Lanceur.Repository.Win32Search
{
    internal static class CollectionHelper
    {
        #region Methods

        public static IEnumerable<RepositoryAlias> Cast(this IEnumerable<AppInfo> src)
        {
            var result = new List<RepositoryAlias>();

            foreach (var item in src)
            {
                if (IsValid(item))
                {
                    result.Add(new RepositoryAlias
                    {
                        ExecutionCount = 0,
                        FileName = item.Path,
                        Name = item.Name,
                        Kind = "Flash"
                    });
                }
            }

            return result;
        }

        private static bool IsValid(AppInfo item)
        {
            if (item == null) { return false; }
            else if (item.Name.IsNullOrWhiteSpace()) { return false; }
            else if (File.Exists(item?.Path ?? "")) { return true; }
            else { return true; }
        }

        #endregion Methods
    }
}