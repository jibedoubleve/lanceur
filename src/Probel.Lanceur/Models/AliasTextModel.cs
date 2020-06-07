using Probel.Lanceur.Core.Entities;
using System.Collections.Generic;
using System.Windows.Media;

namespace Probel.Lanceur.Models
{
    public static class AliasTextModelHelper
    {
        #region Methods

        public static IEnumerable<AliasTextModel> Cast(this IEnumerable<AliasText> src)
        {
            var dst = new List<AliasTextModel>();
            foreach (var item in src)
            {
                dst.Add(new AliasTextModel(item));
            }
            return dst;
        }

        #endregion Methods
    }

    public class AliasTextModel : AliasText
    {
        #region Constructors

        public AliasTextModel(AliasText src)
        {
            ExecutionCount = src.ExecutionCount;
            FileName = src.FileName;
            Image = null;
            Kind = src.Kind;
            Name = src.Name;
        }

        #endregion Constructors

        #region Properties

        public ImageSource Image { get; set; }

        #endregion Properties
    }
}