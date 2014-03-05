
namespace Histria.Sys
{
    public static class L
    {
        public static string T(string value)
        {
            return value;
        }

        public static string T(string fmt, params object[] args)
        {
            return T(string.Format(fmt, args));
        }
    }
   
}
