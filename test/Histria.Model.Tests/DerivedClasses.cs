using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Histria.Model;

namespace Histria.Model.Tests.ModelToTest
{

    public class CParent : BaseModel, IPersistentObj
    {
        public virtual string ParentMember { get; set; }
    }

    public class CC1 : CParent
    {
        public virtual string CC1Member { get; set; }
    }

    public class CC2 : CParent
    {
        public virtual string CC2Member { get; set; }
    }

    public class CC3 : CC1
    {
        public virtual string CC3Member { get; set; }
    }

    //----------------------------------------
    [Db("XXX")]
    [Index("I1,I2")]
    public class CP1 : BaseModel, IPersistentObj
    {
        [Db("M")]
        public virtual string ParentMember { get; set; }
        public virtual string I1 { get; set; }
        public virtual string I2 { get; set; }
    }
    [Index("I1,I3")]
    public class CC4 : CP1
    {
        public virtual string I3 { get; set; }
    }


    //----------------------------------------
    // Invalid table name
    //----------------------------------------
    [Db("XXX")]
    public class CP2 : BaseModel, IPersistentObj
    {

    }

    [Db("YYY")]
    public class CC5 : CP2
    {
    }

    ///----------------------------------------
    // Invalid Primary key
    //----------------------------------------
    [Db("XXX")]
    [PrimaryKey("Id")]
    public class CP3 : BaseModel, IPersistentObj
    {
        public virtual string Id { get; set; }
    }

    [PrimaryKey("Name")]
    public class CC6 : CP3
    {
        public virtual string Name { get; set; }
    }


    ///----------------------------------------
    // Check PK equals
    //----------------------------------------
    [Db("XXX")]
    [PrimaryKey("Id")]
    public class CP4 : BaseModel, IPersistentObj
    {
        public virtual string Id { get; set; }
    }

    public class CC7 : CP4
    {
        public virtual string Name { get; set; }
    }

    ///----------------------------------------
    // Enums
    //----------------------------------------
    [Db(EnumStoredAsString = true)]
    public enum TypeYesNo
    {
        Yes, No
    }
    public class EnuUsed : BaseModel, IPersistentObj
    {
        public virtual TypeYesNo IsRed { get; set; }
    }




}
