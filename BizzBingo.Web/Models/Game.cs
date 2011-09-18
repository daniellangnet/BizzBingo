using System;
using System.Collections.Generic;

namespace BizzBingo.Web.Models
{
    public class Game
    {
        public Guid Id { get; set; }
        public int Number { get; set; }
        public Guid WinnerUserId { get; set; }
        public IList<Player> Players { get; set; }
    }
}