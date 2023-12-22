using WordSearch.Models;
using System;
using WordSearch.Services.Contracts;

namespace WordSearch.Services
{
    public class GridService : IGridService
    {
        private GridCellModel[,] grid;

        public void InitializeGrid(int width, int height)
        {
            Console.WriteLine($"Initializing grid with width: {width}, height: {height}");
            grid = new GridCellModel[height, width];
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    grid[i, j] = new GridCellModel { Letter = '.', IsSelected = false, IsCorrectWord = false };
                }
            }
        }

        public int CalculateMaxWordLengthForUI(List<string> words)
        {
            if (words == null || !words.Any())
            {                
                return 0; 
            }
            int maxWordLength = words.Max(w => w.Length);
            int pixelPerCell = 20;
            return maxWordLength * pixelPerCell;            
        }

        public GridCellModel[,] GetGrid()
        {
            Console.WriteLine("Getting grid");
            return grid;
        }

        public GridCellModel GetCell(int row, int col)
        {
            if (IsWithinBounds(row, col))
            {
                return grid[row, col];
            }
            else
            {
                return null; 
            }
        }

        public void ToggleCellSelection(int row, int col)
        {
            Console.WriteLine($"Toggling cell selection at Row: {row}, Column: {col}");
            if (IsWithinBounds(row, col))
            {
                grid[row, col].IsSelected = !grid[row, col].IsSelected;
            }
        }

        public string GetCellClass(int row, int col)
        {
            Console.WriteLine($"Getting cell class for Row: {row}, Column: {col}");
            if (!IsWithinBounds(row, col)) return string.Empty;

            if (grid[row, col].IsCorrectWord) return "correct-word";
            if (grid[row, col].IsSelected) return "selected-cell";
            return string.Empty;
        }

        public char GetCellValue(int row, int col)
        {
            Console.WriteLine($"Getting cell value for Row: {row}, Column: {col}");
            return IsWithinBounds(row, col) ? grid[row, col].Letter : '\0';
        }

        public bool IsCellEmpty(int row, int col)
        {
            Console.WriteLine($"Checking if cell is empty at Row: {row}, Column: {col}");
            return IsWithinBounds(row, col) && grid[row, col].Letter == '.';
        }

        public bool IsCellSelected(int row, int col)
        {
            Console.WriteLine($"Checking if cell is selected at Row: {row}, Column: {col}");
            return IsWithinBounds(row, col) && grid[row, col].IsSelected;
        }

        public bool IsCellPartOfCorrectWord(int row, int col)
        {
            Console.WriteLine($"Checking if cell is part of a correct word at Row: {row}, Column: {col}");
            return IsWithinBounds(row, col) && grid[row, col].IsCorrectWord;
        }

        public void SetCellValue(int row, int col, char letter)
        {
            Console.WriteLine($"Setting cell value at Row: {row}, Column: {col} to '{letter}'");
            if (IsWithinBounds(row, col))
            {
                grid[row, col].Letter = letter;
            }
        }

        public void ResetGrid()
        {
            Console.WriteLine("Resetting grid");
            if (grid == null) return;

            int height = grid.GetLength(0);
            int width = grid.GetLength(1);
            InitializeGrid(width, height);
        }

        private bool IsWithinBounds(int row, int col)
        {
            int height = grid.GetLength(0);
            int width = grid.GetLength(1);
            bool withinBounds = row >= 0 && row < height && col >= 0 && col < width;
            Console.WriteLine($"Checking if cell is within bounds: {withinBounds}");
            return withinBounds;
        }

        public (int width, int height) RecalculateGridSize(List<string> words)
        {
            
            int maxWordLength = words.Max(w => w.Length);
                        
            int totalCharacters = words.Sum(w => w.Length);
                        
            int minGridSize = (int)Math.Ceiling(Math.Sqrt(totalCharacters));
                        
            int extraColumns = maxWordLength;
            int extraRows = maxWordLength;
                        
            int width = minGridSize + extraColumns;
            int height = minGridSize + extraRows;

            return (width, height);
        }

        public bool IsWordCorrectlyPlaced(string word, GridCellModel[,] grid)
        {
            int height = grid.GetLength(0);
            int width = grid.GetLength(1);

            // Check rows
            for (int i = 0; i < height; i++)
            {
                string row = string.Concat(Enumerable.Range(0, width).Select(j => grid[i, j].Letter));
                if (row.Contains(word) || row.Contains(new string(word.Reverse().ToArray())))
                    return true;
            }

            // Check columns
            for (int j = 0; j < width; j++)
            {
                string column = string.Concat(Enumerable.Range(0, height).Select(i => grid[i, j].Letter));
                if (column.Contains(word) || column.Contains(new string(word.Reverse().ToArray())))
                    return true;
            }

            // Check diagonals (up and down)
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    string diagonalDown = "";
                    string diagonalUp = "";
                    for (int k = 0; k < word.Length; k++)
                    {
                        if (i + k < height && j + k < width)
                            diagonalDown += grid[i + k, j + k].Letter;

                        if (i - k >= 0 && j + k < width)
                            diagonalUp += grid[i - k, j + k].Letter;
                    }
                    if (diagonalDown.Contains(word) || diagonalDown.Contains(new string(word.Reverse().ToArray())))
                        return true;
                    if (diagonalUp.Contains(word) || diagonalUp.Contains(new string(word.Reverse().ToArray())))
                        return true;
                }
            }

            return false;
        }

        public bool IsGridSizeSufficient(GridCellModel[,] grid, List<string> words)
        {
            Console.WriteLine("Checking if grid size is sufficient.");

            int maxWordLength = words.Max(w => w.Length);
            int gridSize = Math.Max(grid.GetLength(0), grid.GetLength(1));
                        
            return gridSize >= maxWordLength;
        }

    }
}
