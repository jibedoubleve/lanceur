using Probel.Lanceur.Core.Helpers;
using Probel.Lanceur.Core.Services;
using Probel.Lanceur.Core.ServicesImpl;
using System.Linq;
using Xunit;

namespace Probel.Lanceur.UnitTest
{
    public class Test_SlickRunExtractor
    {
        #region Fields

        private const string _path = @"..\..\Assets\SlickRun.srl";

        #endregion Fields

        #region Methods

        [Fact]
        public void Get_aliases_from_default_file_has_elements()
        {
            var extractor = new SlickRunExtractor();
            var results = extractor.Extract(_path);

            Assert.True(results.Count() > 0);
        }

        [Fact]
        public void Get_aliases_from_default_file_not_null()
        {
            var extractor = new SlickRunExtractor();
            var results = extractor.Extract(_path);

            Assert.NotNull(results);
        }

        #endregion Methods
    }
}