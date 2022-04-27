namespace KamaVerification.UI.Core.Extensions
{
    public static class TypesExtensions
    {
        public static bool StringToBool(this string value) =>
            value.Equals("yes", StringComparison.CurrentCultureIgnoreCase) ||
            value.Equals(bool.TrueString, StringComparison.CurrentCultureIgnoreCase) ||
            value.Equals("1");
    }
}
