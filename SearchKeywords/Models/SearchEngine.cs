namespace SearchKeyWords.Models
{
    public class SearchEngine
    {        
        public string Name { get; set; }

        public int StartPage { get; set; }

        public int LastPage { get; set; }
        
        public string Url { get; set; }
    }
}