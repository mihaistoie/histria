using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Histria
{
    public enum DataTypes
    {
        Unknown,
        uuid,      //uniqueidentifier 
        SmallInt,   //int   
        Int,        //int   
        BigInt,     //bigint
        Bool,       //bit
        Enum,       //smallint
        String,     //varchar or nvarchar
        Number,     //decimal(15,8)     
        Currency,   //Money (decimal(19.4)) 
        Date,       //Date
        Time,       //time
        DateTime,   //DateTime
        Memo,       //Text or Ntext 
        Binary      //binary
    }
}
