using WordSearch.Models;
using static WordSearch.Models.ValidationModels;

namespace WordSearch.Services.Contracts
{
    public interface IValidationService
    {
        List<string> GetValidWordFromGrid();
        void ResetSelection();
        ValidationModels.ValidationResult IsValidWordSelection(List<GridCellModel> selectedCells);
        bool IsValidWord(string word);                
    }
}
