using WordSearch.Models;
using static WordSearch.Services.GridService;

namespace WordSearch.Services.Contracts
{
    public interface IGridService
    {
        void InitializeGrid(int width, int height);
        int CalculateMaxWordLengthForUI(List<string> words);
        GridCellModel[,] GetGrid();
        GridCellModel GetCell(int row, int col);
        void ToggleCellSelection(int row, int col);
        string GetCellClass(int row, int col);
        void SetCellValue(int row, int col, char letter);
        char GetCellValue(int row, int col);
        bool IsCellSelected(int row, int col);
        bool IsCellPartOfCorrectWord(int row, int col);
        bool IsCellEmpty(int row, int col);
        void ResetGrid();
        (int width, int height) RecalculateGridSize(List<string> words);
        bool IsWordCorrectlyPlaced(string word, GridCellModel[,] grid);
        bool IsGridSizeSufficient(GridCellModel[,] grid, List<string> words);
    }

}
