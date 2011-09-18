using System;
using System.Collections.Generic;

namespace BizzBingo.Web.Models
{
    public class Player
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public IList<WordPosition> WordPositions { get; set; } 
    }
}