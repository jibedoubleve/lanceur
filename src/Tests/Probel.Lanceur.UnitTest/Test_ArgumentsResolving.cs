﻿using NSubstitute;
using Probel.Lanceur.Core.Services;
using Probel.Lanceur.Infrastructure.ServicesImpl;
using System.Net;
using Xunit;

namespace Probel.Lanceur.UnitTest
{
    public class Test_ArgumentsResolving
    {
        #region Properties

        public static IDataSourceService DataSource
        {
            get
            {
                var ds = Substitute.For<IDataSourceService>();
                ds.AliasExists(Arg.Any<string>(), Arg.Any<long>()).Returns(true);
                return ds;
            }
        }

        #endregion Properties

        #region Methods

        [Fact]
        public void Can_resolve_raw_text()
        {
            var before = "© #&";
            var after = "© #&";

            var cb = Substitute.For<IClipboardService>();
            cb.GetText().Returns(string.Empty);
            var pm = new ParameterResolver(cb, DataSource);
            var result = pm.Resolve("$I$", before);

            Assert.Equal(after, result);
        }

        [Fact]
        public void Can_urlify_with_not_supported_char()
        {
            var before = "© #&";
            var after = WebUtility.UrlEncode(before);

            var cb = Substitute.For<IClipboardService>();
            cb.GetText().Returns(string.Empty);
            var pm = new ParameterResolver(cb, DataSource);
            var result = pm.Resolve("$W$", before);

            Assert.Equal(after, result);
        }

        [Fact]
        public void Can_urlify_with_only_supported_char()
        {
            var before = "un_deux_trois";
            var after = "un_deux_trois";

            var cb = Substitute.For<IClipboardService>();
            cb.GetText().Returns(string.Empty);
            var pm = new ParameterResolver(cb, DataSource);
            var result = pm.Resolve("$W$", before);

            Assert.Equal(after, result);
        }

        #endregion Methods
    }
}