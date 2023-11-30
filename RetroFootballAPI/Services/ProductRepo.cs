using Microsoft.EntityFrameworkCore;
using RetroFootballAPI.Repositories;
using RetroFootballWeb.Repository;
using RetroFootballAPI.Models;
using RetroFootballAPI.ViewModels;
using RetroFootballAPI.StaticService;

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
                ID = productVM.ID,
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
                Point = productVM.Point,
                UrlMain = await UploadImage.Instance.UploadAsync(productVM.UrlMain),
                UrlSub1 = await UploadImage.Instance.UploadAsync(productVM.UrlSub1),
                UrlSub2 = await UploadImage.Instance.UploadAsync(productVM.UrlSub2),
                UrlThumb = await UploadImage.Instance.UploadAsync(productVM.UrlThumb)
            };

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


        public async Task<Product> Update(Product product)
        {
            _context.Entry(product).State = EntityState.Modified;
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
            var products = _context.Products.ToListAsync();

            return await products;
        }

        public async Task<Product> GetByID(string id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                throw new KeyNotFoundException(id);
            }

            return product;
        }

        public async Task<IEnumerable<Product>> NewArrivals()
        {
            var products = _context.Products
                .OrderBy(p => p.TimeAdded)
                .Take(3)
                .ToListAsync();

            return await products;
        }


        public async Task<IEnumerable<Product>> TopSelling(int month, int year)
        {
            return await _context.OrderDetails
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

        public async Task<IEnumerable<Product>> GetByPrice(decimal min, decimal max, int page, int productPerPage)
        {
            return await _context.Products
                .Where(p => p.Price >= min && p.Price <= max)
                .Skip((page - 1) * productPerPage)
                .Take(productPerPage)
                .ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetProductByPage(int page, int productPerPage)
        {
            return await _context.Products
                .Skip((page - 1) * productPerPage)
                .Take(productPerPage)
                .ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetByCheckBox(List<string> value, int page, int productPerPage)
        {
            return await _context.Products
                .Where(p => value.Any(x => p.Name.Contains(x)))
                .Skip((page - 1) * productPerPage)
                .Take(productPerPage)
                .ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetBySearch(string value, int page, int productPerPage)
        {
            return await _context.Products
                .Where(p => p.Name.ToLower().Contains(value.ToLower()))
                .Skip((page - 1) * productPerPage)
                .Take(productPerPage)
                .ToListAsync();
        }
    }
}
