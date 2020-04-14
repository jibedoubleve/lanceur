using NSubstitute;
using Probel.Lanceur.Core.Services;
using Probel.Lanceur.Core.ServicesImpl;
using Probel.Lanceur.Plugin;
using Xunit;

namespace Probel.Lanceur.UnitTest
{
    public class Test_ArgumentHandling
    {
        #region Fields

        private static readonly IClipboardService _clipboard = Substitute.For<IClipboardService>();

        #endregion Fields

        #region Methods

        [Fact]
        public void Can_split_multiple_parameter_right_cmd()
        {
            GetCmdWithMultipleParameters(out var cmd, out _, out var cmdline);
            Assert.Equal(cmd, cmdline.Command);
        }

        [Fact]
        public void Can_split_multiple_parameter_right_parameters()
        {
            GetCmdWithMultipleParameters(out _, out var param, out var cmdline);
            Assert.Equal(param, cmdline.Parameters);
        }

        [Fact]
        public void Can_split_single_parameter_right_cmd()
        {
            GetCmdWithOneParameter(out var cmd, out _, out var cmdline);
            Assert.Equal(cmd, cmdline.Command);
        }

        [Fact]
        public void Can_split_single_parameter_right_parameters()
        {
            GetCmdWithOneParameter(out _, out var param, out var cmdline);
            Assert.Equal(param, cmdline.Parameters);
        }

        private static void GetCmdWithMultipleParameters(out string cmd, out string param, out Cmdline cmdline)
        {
            cmd = "a";
            param = "un deux trois quatre";
            var mgt = new ParameterResolver(_clipboard);
            cmdline = mgt.Split($"{cmd} {param}");
        }

        private static void GetCmdWithOneParameter(out string cmd, out string param, out Cmdline cmdline)
        {
            cmd = "a";
            param = "un_deux_trois_quatre";
            var mgt = new ParameterResolver(_clipboard);
            cmdline = mgt.Split($"{cmd} {param}");
        }

        #endregion Methods
    }
}