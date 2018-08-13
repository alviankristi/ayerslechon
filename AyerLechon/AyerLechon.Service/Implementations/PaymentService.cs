using AyerLechon.Model;
using AyerLechon.Repo.Domains;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace AyerLechon.Service.Implementations
{
    public class PaymentService
    {
        private AyerLechonContext _context;
        private OrderItemService orderItemService;
        public PaymentService(AyerLechonContext context)
        {
            _context = context;
            orderItemService = new OrderItemService(context);
        }

        public void Create(OrderSummaryViewModel model)
        {
            var orderSummary = new OrderSummary()
            {
                AmountPaid = Convert.ToDecimal(model.Amount),
                CustomerID = model.CustomerId.Value,
                DeliveryAddress = model.DeliveryAddress,
                Notes = model.Notes,
                PaymentOptionId = model.PaymentOptionId.Value,
                PhoneNumber = model.PhoneNumber,
                RegionID = model.RegionId,
                DateNeeded = model.OrderDate,
                OrderDate = DateTime.Now.ToEpochTime()
            };

            if (model.OrderDate < DateTime.Now.ToEpochTime())
            {
                throw new ApplicationException("The Date Needed cannot be less than the current date.");
            }

            if (!string.IsNullOrEmpty(model.DiscountCode))
            {
                var discount = _context.Discounts.FirstOrDefault(a => a.Code == model.DiscountCode);
                if (discount == null)
                {
                    throw new ApplicationException("Voucher code is not found.");
                }
                var isUsed = _context.OrderSummaries.Any(a => a.CustomerID == model.CustomerId && a.DiscountId == discount.DiscountID);
                if (isUsed)
                {
                    throw new ApplicationException("Voucher code has been used.");
                }
                orderSummary.DiscountId = discount.DiscountID;
            }

            foreach (var detail in model.OrderDetails)
            {
                var item = _context.Items.FirstOrDefault(a => a.ItemID == detail.ItemId);
                var orderDetail = new OrderDetail()
                {
                    ItemID = detail.ItemId,
                    SubTotal = detail.Quantity * item.Price,
                    Quantity = detail.Quantity,
                    Price = (double)item.Price
                };
                orderSummary.OrderDetails.Add(orderDetail);
                orderItemService.DecreaseStock(orderDetail);
            }

            orderSummary.PaymentStatusId = PaymentStatusEnum.Unpaid;
            _context.OrderSummaries.Add(orderSummary);

            if (model.RegionId.HasValue)
                UpdateRegion(model.CustomerId.Value, model.RegionId.Value);

            PaymentOption(model.PaymentOptionId.Value, model.CustomerId.Value, model.Amount.Value);

            _context.SaveChanges();
        }

        public IEnumerable<OrderSummaryViewModel> GetPendings(int customerid)
        {
            var path = ConfigurationManager.AppSettings["BaseUrl"] + "api/images/";
            return _context.OrderSummaries.Include("Customer").Include("OrderDetails.Item.Image").Where(a => a.CustomerID == customerid && a.PaymentStatusId == PaymentStatusEnum.Unpaid && a.PaymentOption.PaymentOptionID == PaymentOptionEnum.CreditCard)
                .Select(a => new OrderSummaryViewModel()
                {
                    OrderSummaryId = a.OrderSummaryID,
                    OrderDate = a.OrderDate,
                    PaymentOptionId = a.PaymentOptionId,
                    Amount = (double)a.AmountPaid,
                    CustomerId = a.CustomerID,
                    DeliveryAddress = a.DeliveryAddress,
                    DiscountCode = a.Discount.Code,
                    Notes = a.Notes,
                    PhoneNumber = a.PhoneNumber,
                    RegionId = a.RegionID,
                    OrderDetails = a.OrderDetails.Select(b => new OrderDetailViewModel()
                    {
                        ItemId = b.ItemID,
                        Quantity = b.Quantity,
                        Product = new ProductViewModel
                        {
                            Id = b.ItemID,
                            Name = b.Item.Description,
                            ImageUrl = b.Item.Image.HasValue ? path + b.Item.Image : "",
                            Price = (double)b.Item.Price,
                            ReadyStock = b.Item.ReadyStock
                        }
                    }).ToList()
                }).ToList();
        }

        public void SetPaid(int orderSummaryId)
        {
            var orderSummary = _context.OrderSummaries.FirstOrDefault(a => a.OrderSummaryID == orderSummaryId);
            if (orderSummary == null)
            {
                throw new ApplicationException("The Order Summary is not found.");
            }
            if (orderSummary.PaymentStatusId != PaymentStatusEnum.Unpaid)
            {
                throw new ApplicationException("The Order Summary has been paid ");
            }

            _context.OrderSummaries.Attach(orderSummary);
            orderSummary.PaymentStatusId = null;
            _context.SaveChanges();
        }


        private void UpdateRegion(int userId, int regionId)
        {
            var customer = _context.Customers.FirstOrDefault(a => a.CustomerID == userId);

            if (customer == null)
            {
                throw new ApplicationException("The Customer is not found.");
            }
            _context.Customers.Attach(customer);
            customer.RegionID = regionId;
        }

        private void PaymentOption(int paymentOptionId, int userId, double amount)
        {
            switch (paymentOptionId)
            {
                case PaymentOptionEnum.CreditLine:
                    var user = _context.Customers.FirstOrDefault(a => a.CustomerID == userId);
                    if (user == null)
                    {
                        throw new ApplicationException("The customer is not found.");

                    }
                    var creaditAmount = user.CreditLimit - Convert.ToDecimal(amount);
                    if (creaditAmount < 0)
                    {
                        throw new ApplicationException("The credit limit is not enough to pay the order.");
                    }
                    else
                    {
                        _context.Customers.Attach(user);
                        user.CreditLimit = creaditAmount;
                    }
                    break;
            }
        }

    }
}
