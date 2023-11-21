namespace RetroFootballAPI.ViewModels
{
    public class ProductVM
    {
        public string? ID { get; set; }
        public string? Name { get; set; }
        public string? Club { get; set; }
        public string? Nation { get; set; }
        public string? Season { get; set; }
        public decimal Price { get; set; }
        public int SizeS { get; set; }
        public int SizeM { get; set; }
        public int SizeL { get; set; }
        public int SizeXL { get; set; }
        public string? Status { get; set; }
        public DateTime TimeAdded { get; set; }
        public string? Description { get; set; }
        public double Point { get; set; }
        public string? UrlMain { get; set; }
        public string? UrlSub1 { get; set; }
        public string? UrlSub2 { get; set; }
        public string? UrlThumb { get; set; }
    }
}
