using RetroFootballAPI.Models;

namespace RetroFootballAPI.ViewModels
{
    public class RecommendationVM
    {
        public float Score { get; set; }
        public Product Product { get; set; }

        public RecommendationVM(float score, Product product)
        {
            Score = score;
            Product = product;
        }
    }
}
