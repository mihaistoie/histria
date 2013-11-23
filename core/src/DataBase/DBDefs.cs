using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sikia.DataBase
{

    
    public enum DBProvider
    {
       mssql, ora, jet, mysql 
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


