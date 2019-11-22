using NSubstitute;
using Probel.Lanceur.Core.Services;
using Probel.Lanceur.Core.ServicesImpl;
using Xunit;

namespace Probel.Lanceur.UnitTest
{
    public class Test_ArgumentsResolving
    {
        #region Methods

        [Fact]
        public void Can_resolve_raw_text()
        {
            var before = "© #&";
            var after = "© #&";

            var cb = Substitute.For<IClipboardService>();
            cb.GetText().Returns(string.Empty);
            var pm = new ParameterResolver(cb);
            var result = pm.Resolve("$I$", before);

            Assert.Equal(after, result.FileName);
        }

        [Fact]
        public void Can_urlify_with_not_supported_char()
        {
            var before = "© #&";
            var after = "%C2%A9+%23&";

            var cb = Substitute.For<IClipboardService>();
            cb.GetText().Returns(string.Empty);
            var pm = new ParameterResolver(cb);
            var result = pm.Resolve("$W$", before);

            Assert.Equal(after, result.FileName);
        }

        [Fact]
        public void Can_urlify_with_only_supported_char()
        {
            var before = "un_deux_trois";
            var after = "un_deux_trois";

            var cb = Substitute.For<IClipboardService>();
            cb.GetText().Returns(string.Empty);
            var pm = new ParameterResolver(cb);
            var result = pm.Resolve("$W$", before);

            Assert.Equal(after, result.FileName);
        }

        #endregion Methods
    }
}