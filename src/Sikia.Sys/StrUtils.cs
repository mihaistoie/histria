
namespace Sikia.Sys
{
    public static class StrUtils
    {
        public static string TT(string value)
        {
            return value;
        }

        public static string TT(string fmt, params object[] args)
        {
            return TT(string.Format(fmt, args));
        }

        public static string UrlDecode(string value)
        {
            //return System.Net.WebUtility.UrlDecode(value);
            return value;
        }

        public static string UrlEncode(string value)
        {
            //return System.Net.WebUtility.UrlEncode(value);
            return value;
        }
    }
   
}
