namespace WordSearch.Services.Contracts
{
    public interface IWordListService
    {
        void AddWord(string word);
        void RemoveWord(string word);
        List<string> GetWords();
        void ClearWords();
    }

}
