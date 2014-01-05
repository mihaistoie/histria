using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sikia.DataBase
{

    
    public enum DbProtocol
    {
       unknown, mssql, ora, jet, mysql 
    }


    public enum DBType
    {
        Int,
        Enum, 
        Varchar,
        Float, 
        Currency,
        DateTime,
        Memo, 
        Binary
    }
    
}


