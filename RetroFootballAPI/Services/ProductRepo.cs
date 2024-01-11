using Humanizer.Localisation;
using Microsoft.EntityFrameworkCore;
using RetroFootballAPI.Models;
using RetroFootballAPI.Repositories;
using RetroFootballAPI.StaticService;
using RetroFootballAPI.StaticServices;
using RetroFootballAPI.ViewModels;
using RetroFootballWeb.Repository;
using System.Text.RegularExpressions;

namespace RetroFootballAPI.Services
{
    public class ProductRepo : IProductRepo
    {
        private readonly DataContext _context;

        public ProductRepo(DataContext context)
        {
            _context = context;
        }


        public async Task<Product> Add(ProductVM productVM)
        {
            var product = new Product
            {
                ID = await AutoID(),
                Name = productVM.Name,
                Club = productVM.Club,
                Nation = productVM.Nation,
                Season = productVM.Season,
                Price = productVM.Price,
                SizeS = productVM.SizeS,
                SizeM = productVM.SizeM,
                SizeL = productVM.SizeL,
                SizeXL = productVM.SizeXL,
                Status = productVM.Status,
                TimeAdded = DateTime.Now,
                Description = productVM.Description,
                Point = 0,
                Sold = 0,
                UrlMain = await UploadImage.Instance.UploadAsync(productVM.UrlMain),
                UrlSub1 = await UploadImage.Instance.UploadAsync(productVM.UrlSub1),
                UrlSub2 = await UploadImage.Instance.UploadAsync(productVM.UrlSub2),
                UrlThumb = await UploadImage.Instance.UploadAsync(productVM.UrlThumb)
            };

            if (product.Club != "None")
            {
                product.GroupName = GetGroupName(product.Club, true);
            }
            else
            {
                product.GroupName = GetGroupName(product.Nation, false);
            }

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return product;
        }


        public async Task<Product> Delete(string id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                throw new KeyNotFoundException(id);
            }

            _context.Remove(product);

            await _context.SaveChangesAsync();

            return product;
        }


        public async Task<Product> Update(string productID, ProductVM productVM)
        {
            var product = await _context.Products.FindAsync(productID);

            if (product == null)
            {
                throw new KeyNotFoundException();
            }

            product.Name = productVM.Name;
            product.Club = productVM.Club;
            product.Nation = productVM.Nation;
            product.Season = productVM.Season;
            product.Price = productVM.Price;
            product.SizeS = productVM.SizeS;
            product.SizeM = productVM.SizeM;
            product.SizeL = productVM.SizeL;
            product.SizeXL = productVM.SizeXL;
            product.Status = productVM.Status;
            product.Description = productVM.Description;
            product.UrlMain = await UploadImage.Instance.UploadAsync(productVM.UrlMain);
            product.UrlSub1 = await UploadImage.Instance.UploadAsync(productVM.UrlSub1);
            product.UrlSub2 = await UploadImage.Instance.UploadAsync(productVM.UrlSub2);
            product.UrlThumb = await UploadImage.Instance.UploadAsync(productVM.UrlThumb);

            _context.Products.Update(product);
            await _context.SaveChangesAsync();

            return product;
        }


        public async Task<IEnumerable<Product>> BestSeller()
        {
            var productIDs = _context.OrderDetails
                .GroupBy(od => od.ProductID)
                .Select(g => new
                {
                    ID = g.Key,
                    Total = g.Sum(od => od.Quantity)
                })
                .OrderByDescending(x => x.Total)
                .Take(3)
                .ToList();

            if (productIDs.Count < 1)
            {
                throw new ArgumentNullException("Not selling yet!");
            }

            while (productIDs.Count < 3)
            {
                productIDs.Add(productIDs[productIDs.Count - 1]);
            }

            return await _context.Products
                .Where(p =>
                p.ID == productIDs[0].ID ||
                p.ID == productIDs[1].ID ||
                p.ID == productIDs[2].ID)
                .ToListAsync();
        }


        public async Task<IEnumerable<Product>> GetAll()
        {
            var products = await _context.Products.ToListAsync();

            foreach (var product in products)
            {
                product.Point = await GetAvgPoint(product.ID);
                product.Sold = await Sold(product.ID);
            }

            return products;
        }

        public async Task<Product> GetByID(string id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                throw new KeyNotFoundException(id);
            }

            product.Point = await GetAvgPoint(product.ID);
            product.Sold = await Sold(product.ID);

            return product;
        }

        public async Task<IEnumerable<Product>> NewArrivals()
        {
            var products = await _context.Products
                .OrderByDescending(p => p.TimeAdded)
                .Take(3)
                .ToListAsync();

            foreach (var product in products)
            {
                product.Point = await GetAvgPoint(product.ID);
                product.Sold = await Sold(product.ID);
            }

            return products;
        }


        public async Task<IEnumerable<Product>> TopSelling(int month, int year)
        {
            var products = await _context.OrderDetails
                .Where(o => o.Order.TimeCreate.Month == month &&
                            o.Order.TimeCreate.Year == year).
                           GroupBy(od => od.ProductID)
                .Select(g => new
                {
                    ProductID = g.Key,
                    TotalQuantity = g.Sum(od => od.Quantity)
                })
                .OrderByDescending(g => g.TotalQuantity)
                .Take(3)
                .Join(_context.Products,
                      detail => detail.ProductID,
                      product => product.ID,
                      (detail, product) => product)
                .ToListAsync();

            foreach (var product in products)
            {
                product.Point = await GetAvgPoint(product.ID);
                product.Sold = await Sold(product.ID);
            }

            return products;
        }

        public async Task<IEnumerable<Product>> GetByCategory(string cate, string value, int page, int productPerPage)
        {
            IEnumerable<Product> products;

            if (cate.Equals("Club", StringComparison.OrdinalIgnoreCase))
            {
                products = await _context.Products
                    .Where(p => p.Club == value)
                    .ToListAsync();
            }
            else
            if (cate.Equals("Nation", StringComparison.OrdinalIgnoreCase))
            {
                products = await _context.Products
                    .Where(p => p.Nation == value)
                    .ToListAsync();
            }
            else
            {
                products = await _context.Products
                    .Where(p => p.Season == value)
                    .ToListAsync();
            }

            return products
                .Skip((page - 1) * productPerPage)
                .Take(productPerPage);
        }

        public async Task<IEnumerable<Product>> GetBySearch(string value, int page, int productPerPage)
        {
            return await _context.Products
                .Where(p => p.Name.ToLower().Contains(value.ToLower()))
                .Skip((page - 1) * productPerPage)
                .Take(productPerPage)
                .ToListAsync();
        }

        public async Task<IEnumerable<Product>> FilterBy(
            List<string> names,
            List<string> seasons,
            List<string> groups,
            bool club = true,
            bool nation = true,
            decimal minPrice = 0,
            decimal maxPrice = 10000000,
            string sortBy = "Name",
            bool descending = false,
            bool sizeS = false,
            bool sizeM = false,
            bool sizeL = false,
            bool sizeXL = false,
            int page = 1,
            int productPerPage = 8)
        {
            var filterProducts = await _context.Products.ToListAsync();

            if (club || nation)
            {
                if (club && !nation)
                {
                    filterProducts = filterProducts
                        .Where(p => p.Club != "None")
                        .ToList();
                }
                else if (!club && nation)
                {
                    filterProducts = filterProducts
                        .Where(p => p.Nation != "None")
                        .ToList();
                }

                if (groups.Count == 1 && string.IsNullOrEmpty(groups[0]))
                {

                }
                else
                if (groups.Count > 0)
                {
                    filterProducts = filterProducts
                        .Where(p => groups.Contains(p.GroupName))
                        .ToList();
                }
            }

            filterProducts = filterProducts
                .Where(p => p.Price >= minPrice && p.Price <= maxPrice)
                .ToList();

            if (sizeS)
            {
                filterProducts = filterProducts
                    .Where(p => p.SizeS > 0)
                    .ToList();
            }
            if (sizeM)
            {
                filterProducts = filterProducts
                    .Where(p => p.SizeM > 0)
                    .ToList();
            }
            if (sizeL)
            {
                filterProducts = filterProducts
                    .Where(p => p.SizeL > 0)
                    .ToList();
            }
            if (sizeXL)
            {
                filterProducts = filterProducts
                    .Where(p => p.SizeXL > 0)
                    .ToList();
            }

            switch (sortBy)
            {
                case "Newest":
                    filterProducts = filterProducts
                            .OrderByDescending(p => p.TimeAdded)
                            .ToList();
                    break;
                case "Name":
                    if (descending)
                    {
                        filterProducts = filterProducts
                            .OrderByDescending(p => p.Name)
                            .ToList();
                    }
                    else
                    {
                        filterProducts = filterProducts
                            .OrderBy(p => p.Name)
                            .ToList();
                    }
                    break;
                case "Price":
                    if (descending)
                    {
                        filterProducts = filterProducts
                            .OrderByDescending(p => p.Price)
                            .ToList();
                    }
                    else
                    {
                        filterProducts = filterProducts
                            .OrderBy(p => p.Price)
                            .ToList();
                    }
                    break;
            }

            var result = new List<Product>();

            if (names.Count > 0)
            {
                foreach (var item in filterProducts)
                {
                    if (names.Any(x => item.Name.Contains(x)))
                    {
                        result.Add(item);
                    }
                }

                filterProducts = result;
            }

            if (seasons.Count > 0)
            {
                result.Clear();

                foreach (var item in filterProducts)
                {
                    if (seasons.Any(x => item.Season.Contains(x)))
                    {
                        result.Add(item);
                    }
                }

                filterProducts = result;
            }

            foreach (var product in filterProducts)
            {
                product.Point = await GetAvgPoint(product.ID);
                product.Sold = await Sold(product.ID);
            }

            if (sortBy == "TopSelling")
            {
                filterProducts = filterProducts
                    .Where(p => p.Sold > 0)
                    .OrderByDescending(p => p.Sold)
                    .ToList(); 
            }

            if (sortBy == "Sold")
            {
                if (descending)
                {
                    filterProducts = filterProducts
                        .OrderByDescending(p => p.Sold)
                        .ToList();
                }
                else
                {
                    filterProducts = filterProducts
                        .OrderBy(p => p.Sold)
                        .ToList();
                }
            }

            return filterProducts
                .Skip((page - 1) * productPerPage)
                .Take(productPerPage)
                .ToList();
        }

        public async Task<double> GetAvgPoint(string productID)
        {
            try
            {
                return await _context.Feedbacks.Where(f => f.ProductID == productID)
                    .Select(f => f.Point)
                    .AverageAsync();
            }
            catch
            {
                return 0;
            }
        }

        public async Task<int> Sold(string productID)
        {
            return await _context.OrderDetails
                .Where(p => p.ProductID == productID)
                .Select(p => p.Quantity)
                .SumAsync();
        }

        private async Task<string> AutoID()
        {
            var ID = "PD0001";

            var maxID = await _context.Products
                .OrderByDescending(v => v.ID)
                .Select(v => v.ID)
                .FirstOrDefaultAsync();

            if (string.IsNullOrEmpty(maxID))
            {
                return ID;
            }

            ID = "PD";

            var numeric = Regex.Match(maxID, @"\d+").Value;

            numeric = (int.Parse(numeric) + 1).ToString();

            while (ID.Length + numeric.Length < 6)
            {
                ID += '0';
            }

            return ID + numeric;
        }

        public async Task<IEnumerable<string>> GetByGroup(List<string> groups)
        {
            var products = await _context.Products.ToListAsync();

            var result = new List<string>();

            foreach (var product in products)
            {
                if (groups.Contains(product.GroupName))
                {
                    if (product.Club != "None")
                    {
                        if (!result.Contains(product.Club))
                        {
                            result.Add(product.Club);
                        }
                        //result.Add(product.Club);
                    }
                    else
                    {
                        if (!result.Contains(product.Nation))
                        {
                            result.Add(product.Nation);
                        }
                        //result.Add(product.Nation);
                    }
                }
            }

            return result;
        }
        public async Task<List<RecommendationVM>> RecommendProducts(string customerId)
        {
            var result = new List<RecommendationVM>();
            var allProducts = await _context.Products.ToListAsync();

            var checkBought = await _context.Orders.Where(o => o.CustomerID == customerId).ToListAsync();
            if (checkBought == null || checkBought.Count <= 0)
            {
                var favoriteTeamsName = await _context.FavoriteTeams.Where(c => c.CustomerID == customerId).ToListAsync();

                foreach (var team in favoriteTeamsName)
                {
                    var pds = allProducts.Where(p => p.Club?.ToLower() == team.TeamName?.ToLower()).ToList();

                    foreach (var pd in pds)
                    {
                        result.Add(new RecommendationVM(0, pd));
                    }

                    pds = allProducts.Where(p => p.Nation?.ToLower() == team.TeamName?.ToLower()).ToList();
                    foreach (var pd in pds)
                    {
                        result.Add(new RecommendationVM(0, pd));
                    }
                }

                if (result.Count <= 0)
                {
                    var rand = new Random();
                    var maxID = int.Parse(allProducts[allProducts.Count - 1].ID.Substring(2));
                    for (int i = 0; i < 8; i++)
                    {
                        var index = rand.Next(1, maxID + 1);
                        var rec = new RecommendationVM(0, allProducts[index]);

                        if (!result.Contains(rec))
                        {
                            result.Add(rec);
                        }
                    }
                }
            }
            else
            {
                var customerID = int.Parse(customerId.Substring(2));
                MLModel.ModelInput sampleData;
                foreach (var product in allProducts)
                {
                    sampleData = new MLModel.ModelInput()
                    {
                        CustomerID = customerID,
                        ProductID = int.Parse(product.ID.Substring(2))
                    };
                    var predictionResult = MLModel.Predict(sampleData);
                    result.Add(new RecommendationVM(predictionResult.Score, product));
                }
                result = result.OrderByDescending(x => x.Score).ToList();
            }
            
            return result.Take(8).ToList();
        }

        public string GetGroupName(string name, bool club)
        {
            if (club)
            {
                if (Teams.PremierLeagueTeams.Contains(name))
                {
                    return "PremierLeague";
                }

                if (Teams.LaLigaTeams.Contains(name))
                {
                    return "Laliga";
                }

                if (Teams.BundesligaTeams.Contains(name))
                {
                    return "Bundesliga";
                }

                if (Teams.SerieATeams.Contains(name))
                {
                    return "SerieA";
                }
                
                if (Teams.Ligue1Teams.Contains(name))
                {
                    return "Ligue1";
                }
                return "VLeague";
            }
            else
            {
                if (Teams.EuropeanNationalTeams.Contains(name))
                {
                    return "Europe";
                }

                if (Teams.AsianNationalTeams.Contains(name))
                {
                    return "Asia";
                }

                if (Teams.SouthAmericanNationalTeams.Contains(name))
                {
                    return "SouthAmerica";
                }

                return "NorthAmerica";
            }
        }
    }
}
