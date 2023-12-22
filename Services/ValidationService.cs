using System.Text;
using WordSearch.Models;
using WordSearch.Services.Contracts;

namespace WordSearch.Services
{
    public class ValidationService : IValidationService
    {
        private readonly IGridService _gridService;
        private readonly IWordListService _wordListService;

        public ValidationService(IGridService gridService, IWordListService wordListService)
        {
            _gridService = gridService;
            _wordListService = wordListService;
        }
        public List<string> GetValidWordFromGrid()
        {
            var grid = _gridService.GetGrid();
            List<string> validWords = new List<string>();
            int height = grid.GetLength(0);
            int width = grid.GetLength(1);

            // Check horizontally and vertically
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    // Horizontal words
                    ExtractWordsFromPosition(i, j, 0, 1, validWords);

                    // Vertical words
                    ExtractWordsFromPosition(i, j, 1, 0, validWords);
                }
            }

            // Check diagonally (down-right and up-right)
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    // Diagonal down-right
                    ExtractWordsFromPosition(i, j, 1, 1, validWords);

                    // Diagonal up-right
                    if (i - 1 >= 0)
                        ExtractWordsFromPosition(i, j, -1, 1, validWords);
                }
            }

            return validWords.Distinct().ToList();
        }

        private void ExtractWordsFromPosition(int startX, int startY, int deltaX, int deltaY, List<string> validWords)
        {
            var grid = _gridService.GetGrid();
            StringBuilder wordBuilder = new StringBuilder();
            int x = startX;
            int y = startY;

            while (x >= 0 && x < grid.GetLength(0) && y >= 0 && y < grid.GetLength(1))
            {
                wordBuilder.Append(grid[x, y].Letter);
                string currentWord = wordBuilder.ToString();

                if (currentWord.Length >= 2 && IsValidWord(currentWord) && !validWords.Contains(currentWord))
                {
                    validWords.Add(currentWord);
                }

                x += deltaX;
                y += deltaY;
            }
        }


        public bool IsValidWord(string word)
        {
            return !string.IsNullOrWhiteSpace(word) &&
                word.Length >= 2 &&
                word.Length <= 15 &&
                word.All(char.IsLetter);
        }
        public ValidationModels.ValidationResult IsValidWordSelection(List<GridCellModel> selectedCells)
        {
            string formedWord = string.Concat(selectedCells.Select(c => c.Letter));
            var wordsInList = _wordListService.GetWords();
            bool isValid = IsValidWord(formedWord) && wordsInList.Contains(formedWord);

            return new ValidationModels.ValidationResult
            {
                IsValid = isValid,
                FormedWord = formedWord
            };
        }
        public void ResetSelection()
        {
            var grid = _gridService.GetGrid();
            for (int i = 0; i < grid.GetLength(0); i++)
            {
                for (int j = 0; j < grid.GetLength(1); j++)
                {
                    grid[i, j].IsSelected = false;
                }
            }
        }

    }
}
