using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Histria.Db.SqlServer
{
    public class SqlServer
    {
        public enum Engine {
            Desktop = 1,
            Standard = 1,
            Enterprise = 1,
            Azure = 5
        };
        public static int Vers2000 = 8;
        public static int Vers2005 = 9;
        public static int Vers2008 = 10;
        public static int Vers2012 = 11;
    }
}
