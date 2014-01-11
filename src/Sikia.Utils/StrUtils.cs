
namespace Sikia.Utils
{
    public class StrUtils
    {
        public static string Namespace2Name(string value)
        {
            var i = value.LastIndexOf('.');
            if (i > 0) return value.Substring(i + 1);
            return value;
        }
        public static string TT(string value)
        {
            return value;
        }
    }
   
}
