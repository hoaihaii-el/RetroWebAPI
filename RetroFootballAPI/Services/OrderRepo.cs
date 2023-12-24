using RetroFootballAPI.ViewModels;
using Microsoft.EntityFrameworkCore;
using RetroFootballAPI.Models;
using RetroFootballAPI.Repositories;
using RetroFootballWeb.Repository;
using RetroFootballAPI.Resources;
using RetroFootballAPI.StaticServices;
using System.Runtime.InteropServices;

namespace RetroFootballAPI.Services
{
    public class OrderRepo : IOrderRepo
    {
        private readonly DataContext _context;

        public OrderRepo(DataContext context)
        {
            _context = context;
        }

        public async Task<Order> Add(OrderVM orderVM)
        {
            var order = new Order
            {
                CustomerID = orderVM.CustomerID,
                TimeCreate = DateTime.Now,
                Value = orderVM.Value,
                PayMethod = orderVM.PayMethod,
                DeliveryMethod = orderVM.DeliveryMethod,
                Note = orderVM.Note,
                Status = "Pending",
                Shipping = orderVM.Shipping,
                Name = orderVM.Name,
                Phone = orderVM.Phone,
                Address = orderVM.Address,
                IsPaid = false
            };

            switch (orderVM.DeliveryMethod?.ToUpper())
            {
                case "NORMAL":
                    order.DeliveryDate = DateTime.Now.AddDays(4);
                    break;
                case "EXPRESS":
                    order.DeliveryDate = DateTime.Now.AddDays(2);
                    break;
            }

            var customer = await _context.Customers.FindAsync(orderVM.CustomerID);

            if (customer == null)
            {
                throw new KeyNotFoundException();
            }

            order.Customer = customer;
            _context.Orders.Add(order);

            var carts = await _context.Carts
                .Where(c => c.CustomerID == order.CustomerID)
                .ToListAsync();

            foreach (var cart in carts)
            {
                _context.OrderDetails.Add(new OrderDetail
                {
                    OrderID = order.ID,
                    ProductID = cart.ProductID,
                    Size = cart.Size,
                    Quantity = cart.Quantity
                });

                var product = await _context.Products.FindAsync(cart.ProductID);

                if (product != null)
                {
                    switch (cart.Size)
                    {
                        case "SizeS":
                            product.SizeS -= cart.Quantity;
                            if (product.SizeS < 0)
                            {
                                product.SizeS += cart.Quantity;
                                throw new Exception($"Sản phẩm {product.Name} không đủ Size S");
                            }
                            break;
                        case "SizeM":
                            product.SizeM -= cart.Quantity;
                            if (product.SizeM < 0)
                            {
                                product.SizeM += cart.Quantity;
                                throw new Exception($"Sản phẩm {product.Name} không đủ Size M");
                            }
                            break;
                        case "SizeL":
                            product.SizeL -= cart.Quantity;
                            if (product.SizeL < 0)
                            {
                                product.SizeL += cart.Quantity;
                                throw new Exception($"Sản phẩm {product.Name} không đủ Size L");
                            }
                            break;
                        case "SizeXL":
                            product.SizeXL -= cart.Quantity;
                            if (product.SizeXL < 0)
                            {
                                product.SizeXL += cart.Quantity;
                                throw new Exception($"Sản phẩm {product.Name} không đủ Size XL");
                            }
                            break;
                    }

                    _context.Products.Update(product);
                }

                _context.Carts.Remove(cart);
            }

            var voucherDetail = await _context.VoucherApplied.FindAsync(order.CustomerID, order.VoucherID);

            if (voucherDetail != null)
            {
                _context.VoucherApplied.Remove(voucherDetail);
            }

            await _context.SaveChangesAsync();

            return order;
        }

        public async Task<IEnumerable<Order>> GetByCustomer(string customerID, int orderType)
        {
            var status = orderType == 0 ? "All" :
                         orderType == 1 ? "Pending" :
                         orderType == 2 ? "Packaging" :
                         orderType == 3 ? "Delivering" :
                         "Completed";

            if (status == "All")
            {
                return await _context.Orders
                    .Where(o => o.CustomerID == customerID)
                    .ToListAsync();
            }
            else
            {
                return await _context.Orders
                    .Where(o => o.CustomerID == customerID &&
                                o.Status == status)
                    .ToListAsync();
            }
        }

        public async Task<IEnumerable<Order>> GetOrders(
            int orderType = 0,
            int month = 0,
            string customerID = "",
            bool today = false)
        {
            var status = orderType == 0 ? "All" :
                         orderType == 1 ? "Pending" :
                         orderType == 2 ? "Packaging" :
                         orderType == 3 ? "Delivering" :
                         "Completed";

            IEnumerable<Order> orders;

            orders = status == "All"
                ? await _context.Orders.ToListAsync()
                : await _context.Orders
                            .Where(o => o.Status == status)
                            .ToListAsync();

            if (month > 0 && month <= 12)
            {
                orders = orders.Where(o => o.TimeCreate.Month == month);
            }

            if (!string.IsNullOrEmpty(customerID))
            {
                orders = orders.Where(o => o.CustomerID == customerID);
            }

            if (today)
            {
                orders = orders.Where(o => o.TimeCreate.Date == DateTime.Now.Date);
            }

            return orders;
        }

        public async Task<Order> UpdateStatus(int orderID)
        {
            var order = await _context.Orders.FindAsync(orderID);

            if (order == null)
            {
                throw new KeyNotFoundException();
            }

            if (order.Status == "Completed")
            {
                return order;
            }

            if (order.Customer == null)
            {
                var customer = await _context.Customers.FindAsync(order.CustomerID);
                order.Customer = customer;
            }

            if (!string.IsNullOrEmpty(order.VoucherID))
            {
                var voucher = await _context.Voucher.FindAsync(order.VoucherID);
                order.Voucher = voucher;
            }

            var status = new List<string>
            {
                "Pending",
                "Packaging",
                "Delivering",
                "Completed"
            };

            int index = status.IndexOf(order.Status);

            order.Status = status[index + 1];

            var user = await _context.Users.FindAsync(order.CustomerID);

            if (user != null)
            {
                string content = "", subject = "";
                switch (order.Status)
                {
                    case "Packaging":
                        content = await ConfirmEmailContent(order);
                        subject = "[HVPP SPORTS] Đơn hàng của bạn đã được xác nhận!";
                        break;
                    case "Delivering":
                        content = UpdateStatusContent(order);
                        subject = $"[HVPP SPORTS] Cập nhật thông tin giao hàng cho đơn hàng #{order.ID}";
                        break;
                    case "Completed":
                        content = DeliveredContent(order);
                        subject = $"[HVPP SPORTS] Đơn hàng #{order.ID} đã hoàn thành!";

                        order.IsPaid = true;
                        _context.Orders.Update(order);
                        break;
                }

                Gmail.SendEmail(
                    subject,
                    content,
                    new List<string> { user.Email }
                );
            }

            await _context.SaveChangesAsync();

            return order;
        }

        private async Task<string> ConfirmEmailContent(Order order)
        {
            var confirmEmail = HVPPRes.ConfirmEmail;

            var details = await _context.OrderDetails
                .Where(o => o.OrderID == order.ID)
                .ToListAsync();

            decimal subTotal = 0;

            foreach (var detail in details)
            {
                var detailContent = HVPPRes.ProductDetail;

                detailContent = detailContent.Replace("{{ProductName}}", detail.Product.Name);
                detailContent = detailContent.Replace("{{Size}}", detail.Size);
                detailContent = detailContent.Replace("{{Image}}", detail.Product.UrlThumb);
                detailContent = detailContent.Replace("{{Quantity}}", detail.Quantity.ToString());
                detailContent = detailContent.Replace("{{ProductPrice}}", detail.Product.Price.ToString());

                confirmEmail.Insert(confirmEmail.IndexOf("<!-- detail -->"), detailContent + "\n");

                subTotal += detail.Product.Price;
            }

            confirmEmail = confirmEmail.Replace("{{CustomerName}}", order.Customer?.Name);
            confirmEmail = confirmEmail.Replace("{{OrderID}}", order.ID.ToString());
            confirmEmail = confirmEmail.Replace("{{SubTotal}}", subTotal.ToString());
            confirmEmail = confirmEmail.Replace("{{Shipping}}", order.Shipping.ToString());
            if (order.Voucher != null)
            {
                confirmEmail = confirmEmail.Replace("{{Discount}}", order.Voucher.Value.ToString());
            }
            else
            {
                confirmEmail = confirmEmail.Replace("{{Discount}}", "0");
            }
            confirmEmail = confirmEmail.Replace("{{Total}}", order.Value.ToString());
            confirmEmail = confirmEmail.Replace("{{TimeCreated}}", order.TimeCreate.ToShortDateString());
            confirmEmail = confirmEmail.Replace("{{Name}}", order.Name);
            confirmEmail = confirmEmail.Replace("{{Phone}}", order.Phone);
            confirmEmail = confirmEmail.Replace("{{Address}}", order.Address);

            return confirmEmail;
        }

        private string UpdateStatusContent(Order order)
        {
            var content = HVPPRes.UpdateStatus;

            content = content.Replace("{{CustomerName}}", order.Customer?.Name);
            content = content.Replace("{{OrderID}}", order.ID.ToString());
            content = content.Replace("{{OrderStatus}}", "Đang vận chuyển");
            content = content.Replace("{{Total}}", order.Value.ToString());

            return content;
        }

        private string DeliveredContent(Order order)
        {
            var content = HVPPRes.Delivered;

            content = content.Replace("{{CustomerName}}", order.Customer?.Name);
            content = content.Replace("{{OrderID}}", order.ID.ToString());
            content = content.Replace("{{OrderStatus}}", "Đã giao hàng");
            content = content.Replace("{{Total}}", order.Value.ToString());

            return content;
        }

        public async Task<Order> Cancel(int orderID, bool isCancelByAdmin = false)
        {
            var order = await _context.Orders.FindAsync(orderID);

            if (order == null)
            {
                throw new KeyNotFoundException();
            }

            if (order.Status != "Pending" && order.Status != "Packaging" && !isCancelByAdmin)
            {
                throw new Exception("Bạn chỉ có thể hủy Order trước khi được đưa cho đơn vị vận chuyển!");
            }

            if (order.Status == "Completed")
            {
                throw new Exception("Không thể xóa đơn hàng đã hoàn thành!");
            }

            if (order.Status != "Delivering" && isCancelByAdmin)
            {
                throw new Exception("Admin chỉ có thể xóa đơn hàng đang giao!");
            }

            var orderDetails = await _context.OrderDetails
                .Where(o => o.OrderID == order.ID)
                .ToListAsync();

            foreach (var detail in orderDetails)
            {
                var product = await _context.Products.FindAsync(detail.ProductID);

                if (product == null)
                {
                    continue;
                }

                switch (detail.Size)
                {
                    case "SizeS":
                        product.SizeS += detail.Quantity;
                        break;
                    case "SizeM":
                        product.SizeM += detail.Quantity;
                        break;
                    case "SizeL":
                        product.SizeL += detail.Quantity;
                        break;
                    case "SizeXL":
                        product.SizeXL += detail.Quantity;
                        break;
                }

                _context.Products.Update(product);
                _context.OrderDetails.Remove(detail);
            }

            if (!isCancelByAdmin)
            {
                var voucher = await _context.Voucher.FindAsync(order.VoucherID);

                if (voucher == null)
                {
                    return order;
                }

                _context.VoucherApplied.Add(new VoucherApplied
                {
                    VoucherID = order.VoucherID,
                    CustomerID = order.CustomerID
                });

                await _context.SaveChangesAsync();
            }

            return order;
        }

        public async Task<Order> UpdatePaymentStatus(int orderID)
        {
            var order = await _context.Orders.FindAsync(orderID);

            if (order == null)
            {
                throw new KeyNotFoundException();
            }

            order.IsPaid = true;

            _context.Orders.Update(order);
            await _context.SaveChangesAsync();

            return order;
        }
    }
}
