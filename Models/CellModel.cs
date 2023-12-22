namespace WordSearch.Models
{
    public class CellModel
    {
        public int Row { get; set; }
        public int Col { get; set; }
        public char Letter { get; set; }
        public bool IsSelected { get; set; }
    }
}
