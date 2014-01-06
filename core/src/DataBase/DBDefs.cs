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

