namespace WordSearch.Models
{
    public class ValidationModels
    {
        public struct ValidationResult
        {
            public bool IsValid;
            public string ErrorMessage;
            public string FormedWord { get; set; }
        }
    }
}
