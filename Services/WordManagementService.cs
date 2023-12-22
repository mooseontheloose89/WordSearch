using WordSearch.Models;
using WordSearch.Services.Contracts;
using System;
using WordSearch.Enum;
using System.Text;

namespace WordSearch.Services
{
    public class WordManagementService : IWordManagementService
    {
        private Random random = new Random();
        private List<string> wordsInList = new List<string>();
        private List<string> correctWords = new List<string>();
        private List<string> foundWords = new List<string>();
        private const int defaultWidth = 10;
        private const int defaultHeight = 10;

        public void AddWordToList(string word)
        {
            Console.WriteLine($"Adding word '{word}' to list.");
            wordsInList.Add(word);
        }

        public (int width, int height) CalculateGridSize(List<string> words)
        {        
            if (words == null || !words.Any())
            {                
                return (defaultWidth, defaultHeight);
            }

            int maxWordLength = words.Max(w => w.Length);
            int gridSize = (int)Math.Sqrt(words.Sum(w => w.Length)) + 1;
            return (Math.Max(maxWordLength, gridSize), gridSize);
        }


        public bool CheckWordSelection(List<(int row, int col)> selectedCells, GridCellModel[,] grid, List<string> words)
        {
            if (selectedCells == null || !selectedCells.Any())
            {                            
                return false;
            }
            Console.WriteLine("Checking word selection.");

            var firstCell = selectedCells.First();
            var lastCell = selectedCells.Last();
            var direction = DetermineDirection(firstCell, lastCell);

            string selectedWord = ExtractWordFromGrid(selectedCells, grid, direction);
            string reverseSelectedWord = new string(selectedWord.Reverse().ToArray());

            return words.Contains(selectedWord) || words.Contains(reverseSelectedWord);
        }

        private string ExtractWordFromGrid(List<(int row, int col)> selectedCells, GridCellModel[,] grid, Direction direction)
        {
            return direction switch
            {
                Direction.Horizontal => string.Concat(selectedCells.Select(c => grid[c.row, c.col].Letter)),
                Direction.Vertical => string.Concat(selectedCells.Select(c => grid[c.row, c.col].Letter)),
                Direction.DiagonalDown => string.Concat(selectedCells.Select(c => grid[c.row++, c.col++].Letter)),
                Direction.DiagonalUp => string.Concat(selectedCells.Select(c => grid[c.row--, c.col++].Letter)),
                _ => string.Empty,
            };
        }

        private Direction DetermineDirection((int row, int col) firstCell, (int row, int col) lastCell)
        {
            if (firstCell.row == lastCell.row) return Direction.Horizontal;
            if (firstCell.col == lastCell.col) return Direction.Vertical;
            if (firstCell.row < lastCell.row) return Direction.DiagonalDown;
            return Direction.DiagonalUp;
        }

        public void FillEmptySpaces(GridCellModel[,] grid)
        {
            Console.WriteLine("Filling empty spaces.");
            for (int i = 0; i < grid.GetLength(0); i++)
            {
                for (int j = 0; j < grid.GetLength(1); j++)
                {
                    if (grid[i, j].Letter == '.')
                    {
                        grid[i, j].Letter = (char)('A' + random.Next(0, 26));
                    }
                }
            }
        }

        public void PlaceWords(List<string> words, GridCellModel[,] grid)
        {
            Console.WriteLine("Placing words in the grid.");

            foreach (var word in words)
            {
                bool placed = false;
                while (!placed)
                {
                    int startRow = random.Next(grid.GetLength(0));
                    int startCol = random.Next(grid.GetLength(1));
                    var directions = System.Enum.GetValues(typeof(Direction)).Cast<Direction>().ToList();

                    foreach (var direction in directions.OrderBy(d => random.Next()))
                    {
                        if (CanPlaceWord(word, startRow, startCol, direction, grid))
                        {
                            PlaceWordInGrid(word, startRow, startCol, direction, grid);
                            placed = true;
                            break;
                        }
                    }
                }
            }
        }

        private bool CanPlaceWord(string word, int startRow, int startCol, Direction direction, GridCellModel[,] grid)
        {
            int row = startRow, col = startCol;
            foreach (char c in word)
            {
                if (row < 0 || col < 0 || row >= grid.GetLength(0) || col >= grid.GetLength(1) || grid[row, col].Letter != '.')
                    return false;

                switch (direction)
                {
                    case Direction.Horizontal: col++; break;
                    case Direction.Vertical: row++; break;
                    case Direction.DiagonalDown: row++; col++; break;
                    case Direction.DiagonalUp: row--; col++; break;
                }
            }

            return true;
        }

        private void PlaceWordInGrid(string word, int startRow, int startCol, Direction direction, GridCellModel[,] grid)
        {
            int row = startRow, col = startCol;
            foreach (char c in word)
            {
                grid[row, col].Letter = c;

                switch (direction)
                {
                    case Direction.Horizontal: col++; break;
                    case Direction.Vertical: row++; break;
                    case Direction.DiagonalDown: row++; col++; break;
                    case Direction.DiagonalUp: row--; col++; break;
                }
            }
        }

        public void RandomizeWordPlacement(List<string> words)
        {
            Console.WriteLine("Randomizing word placement.");
            wordsInList = words.OrderBy(x => random.Next()).ToList();
        }

        public void RemoveWordFromList(string word)
        {
            Console.WriteLine($"Removing word '{word}' from list.");
            wordsInList.Remove(word);
        }

        public List<string> RetrieveWordsFromGrid(GridCellModel[,] grid, WordRetrievalCriteria criteria)
        {
            Console.WriteLine("Retrieving words from grid.");
            List<string> retrievedWords = new List<string>();

            int height = grid.GetLength(0);
            int width = grid.GetLength(1);

            
            for (int i = 0; i < height; i++)
            {
                StringBuilder horizontalWord = new StringBuilder();
                StringBuilder verticalWord = new StringBuilder();

                for (int j = 0; j < width; j++)
                {
                    horizontalWord.Append(grid[i, j].Letter);
                    verticalWord.Append(grid[j, i].Letter);
                }

                retrievedWords.Add(horizontalWord.ToString());
                retrievedWords.Add(verticalWord.ToString());
            }

            
            for (int i = 0; i < height; i++)
            {
                StringBuilder diagonalDownWord = new StringBuilder();
                StringBuilder diagonalUpWord = new StringBuilder();

                for (int j = 0; j < width; j++)
                {
                    if (i + j < height)
                        diagonalDownWord.Append(grid[i + j, j].Letter);

                    if (i - j >= 0)
                        diagonalUpWord.Append(grid[i - j, j].Letter);
                }

                retrievedWords.Add(diagonalDownWord.ToString());
                retrievedWords.Add(diagonalUpWord.ToString());
            }
            
            retrievedWords = FilterWordsByCriteria(retrievedWords, criteria);

            return retrievedWords;
        }

        private List<string> FilterWordsByCriteria(List<string> words, WordRetrievalCriteria criteria)
        {
            List<string> filteredWords = new List<string>();

            foreach (var word in words)
            {
                bool shouldInclude = true;

                
                switch (criteria)
                {
                    case WordRetrievalCriteria.Found:
                        shouldInclude = IsWordFound(word);
                        break;
                    case WordRetrievalCriteria.Correct:
                        shouldInclude = IsWordCorrect(word);
                        break;                        
                }

                if (shouldInclude)
                {
                    filteredWords.Add(word);
                }
            }

            return filteredWords;
        }

        private bool IsWordFound(string word)
        {
            if (string.IsNullOrWhiteSpace(word))
                return false;
            if (wordsInList.Contains(word))
            {
                foundWords.Add(word);
                return true;
            }
            return false;
        }

        private bool IsWordCorrect(string word)
        {
            if (string.IsNullOrWhiteSpace(word))
                return false;
            if (wordsInList.Contains(word))
            {
                correctWords.Add(word); 
                return true;
            }
            return false;
        }


        public bool ValidateGrid(GridCellModel[,] grid, List<string> words)
        {
            Console.WriteLine("Validating grid.");
            var gridService = new GridService();
            
            foreach (var word in words)
            {
                if (!gridService.IsWordCorrectlyPlaced(word, grid))
                {
                    Console.WriteLine("Word isn't correctly placed in grid.");
                    return false;
                }
            }
            
            if (!gridService.IsGridSizeSufficient(grid, words))
            {
                Console.WriteLine("Grid size isn't sufficient.");
                var newSize = gridService.RecalculateGridSize(words);
                gridService.InitializeGrid(newSize.width, newSize.height);
                PlaceWords(words, grid);
            }

            return true;
        }
    }
}
