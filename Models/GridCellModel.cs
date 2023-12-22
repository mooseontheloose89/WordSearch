namespace WordSearch.Models
{
    public class GridCellModel
    {
        public char Letter { get; set; }
        public bool IsSelected { get; set; }
        public bool IsCorrectWord { get; set; }
    }
}
