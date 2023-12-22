using WordSearch.Services.Contracts;

namespace WordSearch.Services
{
    public class WordListService : IWordListService
    {
        private List<string> words = new List<string>();
        public void AddWord(string word)
        {
            if(!string.IsNullOrEmpty(word))
            {
                words.Add(word);
            }
            else
            {
                throw new ArgumentException("Word cannot be empty or null.");
            }
        }

        public void ClearWords()
        {
            words.Clear();
        }

        public List<string> GetWords()
        {
            return words;
        }

        public void RemoveWord(string word)
        {
            words.Remove(word);
        }
    }
}
