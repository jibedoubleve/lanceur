namespace Probel.Lanceur.Infrastructure.Extensions
{
    public static class StringExtension
    {
        #region Methods

        public static bool IsNullOrEmpty(this string src) => string.IsNullOrEmpty(src);

        public static bool IsNullOrWhiteSpace(this string src) => string.IsNullOrWhiteSpace(src);

        #endregion Methods
    }
}