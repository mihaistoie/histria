using System.Collections.Generic;
using System.Data.Common;


namespace Sikia.DataBase
{
    public abstract class DatabaseTranslator
    {
        public abstract List<string> Tables(DbConnection connection);
    }

}
