namespace BizzBingo.Web.Models
{
    public class WordPosition
    {
        public string Word { get; set; }
        public int WordId { get; set; }
        public int PositionX { get; set; }
        public int PositionY { get; set; }
        public bool Checked { get; set; }
    }
}