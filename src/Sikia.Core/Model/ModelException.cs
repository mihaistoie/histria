using System;

namespace Sikia.Core.Model
{
    ///<summary>
    ///Represents  errors that occurs during model loading
    ///</summary>      
    public class ModelException : Exception
    {
        #region Properties
        ///<summary>
        ///Tha class  that has raised the exception
        ///</summary>      
        public string ClassName;
        ///<summary>
        ///Tha property  that has raised the exception
        ///</summary>      
        public string PropName;
        #endregion

        #region Construtors
        public ModelException() : base() { }
        public ModelException(string message) : base(message) { }
        public ModelException(string message, string className)
            : base(message)
        {
            ClassName = className;
        }
        public ModelException(string message, string className, string propName)
            : base(message)
        {
            ClassName = className;
            PropName = propName;
        }
        #endregion

    }
}
