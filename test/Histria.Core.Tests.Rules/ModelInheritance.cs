using Histria.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Histria.Core.Tests.Rules.Customers
{
    public enum ShapeType
    {
        Unknown, Shape, Ellipse, Circle, Rectangle
    }

    /// <summary>
    /// Persistent Class (inherited from InterceptedDbObject)
    /// </summary>
    public class Shape : InterceptedDbObject
    {
        /// <summary>
        /// Used for persistent inheritance
        /// </summary>
        [Default(Value = ShapeType.Shape)]
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
}
