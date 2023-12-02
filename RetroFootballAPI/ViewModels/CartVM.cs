namespace RetroFootballAPI.ViewModels
{
    public class CartVM
    {
        public string? CustomerID { get; set; }
        public string? ProductID { get; set; }
        public string? Size { get; set; }
        public int Quantity { get; set; }

        public static List<string> ProductSize = new List<string>
        {
            "SizeS",
            "SizeM",
            "SizeL",
            "SizeXL"
        };
    }
}
