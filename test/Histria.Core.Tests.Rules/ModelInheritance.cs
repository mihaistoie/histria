using Histria.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Histria.Core.Tests.Rules.Customers
{
    public enum ShapeType
    {
        Shape, Ellipse, Circle, Rectangle
    }

    public class Shape : InterceptedObject, IPersistentObj
    {
        /// <summary>
        /// Used for persistent inheritance
        /// </summary>
        public virtual ShapeType Type { get; set; }
    }

    [Inheritance("Type", (int)ShapeType.Ellipse)]
    public class Ellipse : Shape
    {

    }

    [Inheritance("Type", (int)ShapeType.Circle)]
    public class Circle : Ellipse
    {

    }

    [Inheritance("Type", (int)ShapeType.Rectangle)]
    public class Rectangle : Shape
    {

    }
}
