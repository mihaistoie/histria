using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sikia.Db
{

    
    public enum DbProtocol
    {
       unknown, nodb, mssql, ora, mysql 
    }

    //Supported DB types
    public enum DbType
    {
        uuid,      //uniqueidentifier 
        Int,        //int   
        BigInt,     //bigint
        Bool,       //bit
        Enum,       //smallint
        Varchar,    //varchar or nvarchar
        Float,      //decimal(20,8)    
        Currency,   //Money ?
        Date,       //Date
        Time,       //time
        DateTime,   //DateTime
        Memo,       //Text or Ntext 
        Binary      //binary
    }
    
}

