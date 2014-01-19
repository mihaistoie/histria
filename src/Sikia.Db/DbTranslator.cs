using System.Collections.Generic;
using System.Data.Common;


namespace Sikia.Db
{
    public class DbTranslator
    {
        public virtual List<string> Tables(DbSession session)
        {
            return new List<string>();
        }
    }

}
