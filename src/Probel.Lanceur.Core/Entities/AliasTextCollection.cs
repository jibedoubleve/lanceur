using System.Collections.Generic;

namespace Probel.Lanceur.Core.Entities
{
    public static class AliasTextCollection
    {
        public static IEnumerable<AliasText> Refresh(this IEnumerable<AliasText> collection)
        {
            var template = "package:";
            foreach (var item in collection)
            {
                if (item.FileName.ToLower().StartsWith(template))
                {
                    item.IsPackaged = true;

                    var start = template.Length;
                    var length = item.FileName.Length - start;

                    item.UniqueIdentifier = item.FileName.Substring(start, length);
                }
            }
            return collection;
        }
    }
}