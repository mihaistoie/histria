using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Histria.Model
{
    public class ForeignKeyInfo
    {
        public string Field;
        public bool ReadOnly = false;
        public PropInfoItem Prop;
    } 
    public class PKeyInfo
    {
        public string Field;
        public PropInfoItem Prop;
    } 
 
    public class RoleInfoItem
    {
        private List<ForeignKeyInfo> _fkFields;
        private List<PKeyInfo> _pkFields;
        private bool _fkFieldsExist = true;
       
        public int Max { get; internal set; }
        public int Min { get; internal set; }
        
        public Type ClassType { get; internal set; }
        public Relation Type { get; internal set; }
        public PropInfoItem RoleProp { get; internal set; }
        public RoleInfoItem InvRole { get; internal set; }
        public bool IsList { get; internal set; }
        public bool IsRef { get { return !this.IsList; } }
        public string InvRoleName { get; internal set; }
        public string ForeignKey { get; internal set; }
        private RoleInfoItem()
        {

        }
        internal RoleInfoItem(PropInfoItem prop)
        {
            this.RoleProp = prop;
            prop.Role = this;
        }

        ///<summary>
        /// FkFields contains Field and is read only
        ///</summary>   
        public bool HasReadOnlyField(string field) 
        {
            if (this._fkFields != null)
            {
                return this._fkFields.Find((fk) => { return fk.ReadOnly && fk.Field == field; }) != null;
            }
            return false;

        }
        public ClassInfoItem RemoteClass { get; internal set; }

        ///<summary>
        /// Is child (belongs to)
        ///</summary>   
        public bool IsChild{ get; internal set; }

        ///<summary>
        /// Is Parent (the other side of relation is child)
        ///</summary>   
        public bool IsParent { get { return !this.IsChild && this.InvRole != null && this.InvRole.IsChild; } }

        ///<summary>
        /// UseUuid =  (PkFields[0] =='Uuid' && PkFields.Length == 1);
        ///</summary>   
        public bool UseUuid
        {
            get
            {
                return (this.IsRef ? (this.PkFields != null && this._pkFields.Count == 1 && this._pkFields[0].Field == ModelConst.UUID) : true);
            }
        }
        ///<summary>
        /// Foreign key fields are declared in the class ? 
        ///</summary>   
        public bool FkFieldsExist
        {
            get
            {
                return (this.IsRef ? this._fkFieldsExist : true);
            }
            internal set
            {
                if (this.IsRef) this._fkFieldsExist = value;
            }
        }
      
        ///<summary>
        /// Foreign key Fields
        ///</summary>  
        public List<ForeignKeyInfo> FkFields
        {
            get
            {
                return (this.IsRef ? this._fkFields : null);
            }
            internal set
            {
                if (this.IsRef) this._fkFields = value;
            }
        }
        
        ///<summary>
        /// Primary key fields used for this relationship. Can be different than primary key fields of remote class (InvRole.Classtype) 
        ///</List>
        public List<PKeyInfo> PkFields
        {
            get
            {
                return (this.IsRef ? this._pkFields : null);
            }
            internal set
            {
                if (this.IsRef) this._pkFields = value;
            }
        }
        
        ///<summary>
        /// Primary key fields used for this relationship = Primary key fields of remote class  (InvRole.Classtype)  
        ///</summary>
        public bool UsePk { get; internal set; }
    }
}
