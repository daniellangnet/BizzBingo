using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BizzBingo.Web.Models.Home
{
    public class TeaserViewModel
    {
        public Term Term { get; set; }

        /// <summary>
        /// True = Success Stories
        /// False = Fail Stories
        /// null = No stories
        /// </summary>
        public bool? ShowPositiveReactions { get; set; }
    }
}