using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sikia.Model
{
    public class ForeignKeyInfo
    {
        public string Field;
        public bool ReadOnly = false;
    } 

    public class RoleInfoItem
    {
        private List<ForeignKeyInfo> fkFields;
        private bool fkFieldsExist = true;
        private string[] pkFields;
        public int Max { get; set; }
        public int Min { get; set; }
        public Type ClassType { get; set; }
        public Relation Type { get; set; }
        public PropinfoItem RoleProp { get; set; }
        public RoleInfoItem InvRole { get; set; }
        public bool IsList { get; set; }
        public bool IsRef { get { return !IsList; } }
        public string InvRoleName { get; set; }
        public string ForeignKey { get; set; }

        ///<summary>
        /// Is child (belongs to)
        ///</summary>   
        public bool IsChild{ get; set; }

        ///<summary>
        /// Is child (belongs to)
        ///</summary>   
        public bool IsParent { get { return  !IsChild && InvRole != null && InvRole.IsChild; } }

        ///<summary>
        /// UseUuid =  (PkFields[0] =='Uuid' && PkFields.Length == 1);
        ///</summary>   
        public bool UseUuid
        {
            get
            {
                return (IsRef ? (PkFields != null && pkFields.Length == 1 && PkFields[0] == ModelConst.UUID) : true);
            }
        }

        ///<summary>
        /// Foreign key fields are declared in the class ? 
        ///</summary>   
        public bool FkFieldsExist
        {
            get
            {
                return (IsRef ? fkFieldsExist : true);
            }
            set
            {
                if (IsRef) fkFieldsExist = value;
            }
        }
      
        ///<summary>
        /// Foreign key Fields
        ///</summary>  
        public List<ForeignKeyInfo> FkFields
        {
            get
            {
                return (IsRef ? fkFields : null);
            }
            set
            {
                if (IsRef) fkFields = value;
            }
        }
        
        ///<summary>
        /// Primary key fields used for this relationship. Can be different than primary key fields of remote class (InvRole.Classtype) 
        ///</summary>
        public string[] PkFields
        {
            get
            {
                return (IsRef ? pkFields : null);
            }
            set
            {
                if (IsRef) pkFields = value;
            }
        }
        
        ///<summary>
        /// Primary key fields used for this relationship = Primary key fields of remote class  (InvRole.Classtype)  
        ///</summary>
        public bool UsePk { get; set; }
    }
}
