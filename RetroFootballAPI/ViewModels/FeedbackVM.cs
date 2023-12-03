namespace RetroFootballAPI.ViewModels
{
    public class FeedbackVM
    {
        public string? CustomerID { get; set; }
        public string? ProductID { get; set; }
        public string? Comment { get; set; }
        public int Point { get; set; }
        public IFormFile? Media { get; set; }
    }
}
