using System;
using System.Collections.Generic;
using System.Text;
using TypeDB.Annotations;

namespace TypeDB.Tests.Models
{
    [Collection("Player")]
    class Player
    {
        public string Name { get; set; }
        public int Age { get; set; }
    }
}
