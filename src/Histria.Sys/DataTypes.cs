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
        Time,       //Time
        DateTime,   //DateTime
        Memo,       //varchar(max)  or nvarchar(max) 
        Binary      //varbinary(max)
    }

    public static class DataTypesConsts
    {
        //String max size
        public static int MAX_STRING_SIZE = 128;

        //String Codes
        public static int MAX_CODE_SIZE = 64;
        
        
        //Number
        public static int NUMBER_PRECISION = 15;
        public static int NUMBER_SCALE = 8;
        
        //Money
        public static int MONEY_PRECISION = 19;
        public static int MONEY_SCALE = 4;
        
    }
}
