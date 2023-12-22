using WordSearch.Enum;
using WordSearch.Models;
using static WordSearch.Services.GridService;

namespace WordSearch.Services.Contracts
{
    public interface IWordManagementService
    {
        void AddWordToList(string word);
        (int width, int height) CalculateGridSize(List<string> words);
        bool CheckWordSelection(List<(int row, int col)> selectedCells, GridCellModel[,] grid, List<string> words);
        void FillEmptySpaces(GridCellModel[,] grid);
        void PlaceWords(List<string> words, GridCellModel[,] grid);                
        void RandomizeWordPlacement(List<string> words);
        void RemoveWordFromList(string word);
        List<string> RetrieveWordsFromGrid(GridCellModel[,] grid, WordRetrievalCriteria criteria);
        bool ValidateGrid(GridCellModel[,] grid, List<string> words);    

    }

}
