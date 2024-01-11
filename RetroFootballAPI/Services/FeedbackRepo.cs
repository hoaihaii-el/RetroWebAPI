using RetroFootballAPI.ViewModels;
using Microsoft.EntityFrameworkCore;
using RetroFootballAPI.Models;
using RetroFootballAPI.Repositories;
using RetroFootballWeb.Repository;
using RetroFootballAPI.StaticService;

namespace RetroFootballAPI.Services
{
    public class FeedbackRepo : IFeedbackRepo
    {
        private readonly DataContext _context;

        public FeedbackRepo(DataContext context)
        {
            _context = context;
        }


        public async Task<Feedback> Add(FeedbackVM feedbackVM)
        {
            var feedback = new Feedback
            {
                CustomerID = feedbackVM.CustomerID,
                ProductID = feedbackVM.ProductID,
                Comment = feedbackVM.Comment,
                Point = feedbackVM.Point,
                Date = DateTime.Now
            };

            var customer = await _context.Customers.FindAsync(feedbackVM.CustomerID);
            var product = await _context.Products.FindAsync(feedbackVM.ProductID);

            if (customer == null || product == null)
            {
                throw new KeyNotFoundException();
            }

            var orders = await _context.OrderDetails.Where(d => d.OrderID == feedbackVM.OrderID).ToListAsync();
            foreach(var order in orders)
            {
                if (order.ProductID == feedbackVM.ProductID && order.Size == feedbackVM.Size)
                {
                    order.didFeedback = true;
                }
            }

            feedback.Customer = customer;
            feedback.Product = product;
            feedback.Media = await UploadImage.Instance.UploadAsync(feedback.ProductID + feedback.CustomerID + Guid.NewGuid().ToString(), feedbackVM.Media);

            _context.Feedbacks.Add(feedback);

            await _context.SaveChangesAsync();

            return feedback;
        }


        public async Task<IEnumerable<Feedback>> GetAll(string productID)
        {
            var feedbacks =  await _context.Feedbacks
                .Where(f => f.ProductID == productID)
                .ToListAsync();
            foreach(var feedback in feedbacks)
            {
                var customer = await _context.Customers.FindAsync(feedback.CustomerID);
                if (customer != null)
                {
                    feedback.Customer = customer;
                }
            }
            return feedbacks;
        }

        public async Task<IEnumerable<Feedback>> GetAllByCustomerID(string customerID)
        {
            var feedbacks = await _context.Feedbacks
                .Where(f => f.CustomerID == customerID)
                .ToListAsync();
            foreach (var feedback in feedbacks)
            {
                var product = await _context.Products.FindAsync(feedback.ProductID);
                if (product != null)
                {
                    feedback.Product = product;
                }
            }
            return feedbacks;
        }


        public async Task<double> GetAvgPoint(string productID)
        {
            return await _context.Feedbacks.Where(f => f.ProductID == productID)
                .Select(f => f.Point)
                .AverageAsync();
        }


        public async Task<Feedback> Update(FeedbackVM feedbackVM)
        {
            var feedback = await _context.Feedbacks
                .FindAsync(feedbackVM.CustomerID, feedbackVM.ProductID);

            if (feedback == null)
            {
                throw new KeyNotFoundException();
            }

            feedback.Comment = feedbackVM.Comment;
            feedback.Point = feedbackVM.Point;
            feedback.Media = await UploadImage.Instance.UploadAsync(feedback.ProductID + feedback.CustomerID + Guid.NewGuid().ToString(), feedbackVM.Media);

            _context.Feedbacks.Update(feedback);

            await _context.SaveChangesAsync();

            return feedback;
        }
    }
}
