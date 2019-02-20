using System;
using System.Collections.Generic;
using System.Text;

namespace TypeDB.Annotations
{
    public class CollectionAttribute : Attribute
    {
        public string Name { get; private set; }

        public CollectionAttribute()
        {

        }

        public CollectionAttribute(string name)
        {
            this.Name = name;
        }
    }
}
