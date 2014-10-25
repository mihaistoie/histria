using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Histria
{
    public enum DataTypes
    {
        Unknown,    //SQL                          --C#
        uuid,       //uniqueidentifier             --Gid
        SmallInt,   //int                          --byte/char/short/ushort   
        Int,        //int                          --int/uint 
        BigInt,     //bigint                       --long/ulong
        Bool,       //bit                          --bool
        Enum,       //smallint                     --enum 
        String,     //varchar(255)/nvarchar(255)   --string
        Number,     //decimal(15,8)                --Decimal
        Currency,   //Money (decimal(19.4))        --Decimal
        Date,       //Date
        Time,       //time
        DateTime,   //DateTime
        Memo,       //Text or Ntext 
        Binary      //binary
    }
}
