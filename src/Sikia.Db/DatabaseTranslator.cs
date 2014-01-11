using System.Collections.Generic;
using System.Data.Common;


namespace Sikia.Db
{
    public abstract class DatabaseTranslator
    {
        public abstract List<string> Tables(DbSession session);
    }

}
