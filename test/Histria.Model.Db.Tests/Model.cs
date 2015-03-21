﻿using Histria.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Histria.DbModel.Tests
{
    [NoModel]
    public class BaseModel : IClassModel
    {
        public virtual Guid Uuid { get; set; }
    }


    public class Fruit : BaseModel, IPersistentObj
    {
    }

    [PrimaryKey("CodeISO")]
    public class Country : BaseModel, IPersistentObj
    {
        public virtual string CodeISO { get; set; }
    }

    [PrimaryKey("Red,Green,Blue")]
    public class Color : BaseModel, IPersistentObj
    {
        public virtual int Red  { get; set; }
        public virtual int Green { get; set; }
        public virtual int Blue { get; set; }
    }


    public enum ShapeType
    {
        Shape, Ellipse, Circle, Rectangle
    }

    /// <summary>
    /// Persistent Class (inherited from InterceptedDbObject)
    /// </summary>
    public class Shape : BaseModel, IPersistentObj
    {
        /// <summary>
        /// Used for persistent inheritance
        /// </summary>
        public virtual ShapeType Type { get; set; }
    }

    [Inheritance("Type", ShapeType.Ellipse)]
    public class Ellipse : Shape
    {
    }

    [Inheritance("Type", ShapeType.Circle)]
    public class Circle : Ellipse
    {
    }

    [Inheritance("Type", ShapeType.Rectangle)]
    public class Rectangle : Shape
    {
    }

    // <summary>
    /// No Persistent Class 
    /// </summary>
    public class Animal : BaseModel
    {
    }

    /// <summary>
    /// Persistent Class (use Bird data table)
    /// </summary>
    public class Bird : Animal, IPersistentObj
    {
    }
    /// <summary>
    /// Persistent Class (use Mammal data table)
    /// </summary>
    public class Mammal : Animal, IPersistentObj
    {
    }


}
