using System;
using System.Collections.Generic;
using System.Text;
using TypeDB.Annotations;

namespace TypeDB.Tests.Models
{
    [Collection("Gamer")]
    internal class Gamer : Player
    {
        [Id]
        public string Id { get; set; }
    }
}
