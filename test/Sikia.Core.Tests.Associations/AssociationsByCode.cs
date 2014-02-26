using Sikia.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sikia.Core.Tests.Associations
{
    [PrimaryKey("Code")]
    public class Country: InterceptedObject
    {
        public virtual string Code { get; set; }
    }

    [PrimaryKey("CountryCode,CityCode,ZipCode")]
    public class City: InterceptedObject
    {
        public virtual string CountryCode { get; set; }
        public virtual string CityCode { get; set; }
        public virtual string ZipCode { get; set; }
        [Association(Relation.Association, ForeignKey = "CountryCode=Code", Min = 1)]
        public virtual HasOne<Country> Country { get; set; }
    }

    [PrimaryKey("Id")]
    public class Address: InterceptedObject
    {
        public virtual string Id { get; set; }
        public virtual string CountryCode { get; set; }
        public virtual string CityCode { get; set; }
        public virtual string ZipCode { get; set; }
        [Association(Relation.Association, ForeignKey = "CountryCode=Code")]
        public virtual HasOne<Country> Country { get; set; }
        //Wow!! == ? What is this ?
        [Association(Relation.Association, ForeignKey = "CountryCode==CountryCode,CityCode=CityCode,ZipCode=ZipCode")]
        public virtual HasOne<City> City { get; set; }
    }

}
