//using ClothingBrand.Domain.Models;
//using Microsoft.EntityFrameworkCore;
//using System;
//using System.Collections.Generic;
//using System.Diagnostics.Metrics;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace ClothingBrand.Application.Services
//{
//    private readonly ApplicationDbContext _context;
//    private readonly IDiscountService _discountService;
//    private readonly IStripePaymentService _paymentService;

//    public OrderService(ApplicationDbContext context, IDiscountService discountService, IStripePaymentService paymentService)
//    {
//        _context = context;
//        _discountService = discountService;
//        _paymentService = paymentService;
//    }

//    // Create a new order (with both standard and custom clothing orders support)
//    public async Task<OrderDto> CreateOrderAsync(OrderCreateDto dto)
//    {
//        var order = new Order
//        {
//            UserId = dto.UserId,
//            OrderItems = dto.OrderItems.Select(oi => new OrderItem
//            {
//                ProductId = oi.ProductId,
//                Quantity = oi.Quantity,
//                UnitPrice = oi.UnitPrice
//            }).ToList(),
//            TotalAmount = dto.TotalAmount,
//            Status = OrderStatus.Pending,
//            CreatedAt = DateTime.UtcNow,
//            IsCustomOrder = dto.IsCustomOrder
//        };

//        // Handle custom order measurements if applicable
//        if (dto.IsCustomOrder)
//        {
//            order.Measurements = new Measurements
//            {
//                ShoulderWidth = dto.Measurements.ShoulderWidth,
//                ChestCircumference = dto.Measurements.ChestCircumference,
//                WaistCircumference = dto.Measurements.WaistCircumference,
//                // Add other measurement properties
//            };
//        }

//        // Process payment via Stripe
//        var paymentResult = await _paymentService.ProcessPaymentAsync(order.TotalAmount);
//        if (!paymentResult.Success)
//        {
//            throw new InvalidOperationException("Payment failed.");
//        }

//        // Save order to the database
//        _context.Orders.Add(order);
//        await _context.SaveChangesAsync();

//        return new OrderDto
//        {
//            Id = order.Id,
//            UserId = order.UserId,
//            TotalAmount = order.TotalAmount,
//            Status = order.Status,
//            CreatedAt = order.CreatedAt
//        };
//    }

//    // Apply discount to an existing order
//    public async Task<bool> ApplyDiscountAsync(int orderId, string discountCode)
//    {
//        var order = await _context.Orders.FindAsync(orderId);
//        if (order == null) return false;

//        var discount = await _discountService.ValidateDiscountAsync(discountCode);
//        if (discount == null || discount.ExpirationDate < DateTime.UtcNow) return false;

//        order.TotalAmount -= discount.Amount;
//        await _context.SaveChangesAsync();
//        return true;
//    }

//    // Get an order by its ID
//    public async Task<OrderDto> GetOrderByIdAsync(int id)
//    {
//        var order = await _context.Orders.Include(o => o.OrderItems).FirstOrDefaultAsync(o => o.Id == id);
//        if (order == null) return null;

//        return new OrderDto
//        {
//            Id = order.Id,
//            UserId = order.UserId,
//            TotalAmount = order.TotalAmount,
//            Status = order.Status,
//            CreatedAt = order.CreatedAt,
//            OrderItems = order.OrderItems.Select(oi => new OrderItemDto
//            {
//                ProductId = oi.ProductId,
//                Quantity = oi.Quantity,
//                UnitPrice = oi.UnitPrice
//            }).ToList()
//        };
//    }

//    // Retrieve all orders for a user
//    public async Task<IEnumerable<OrderDto>> GetOrdersByUserIdAsync(int userId)
//    {
//        return await _context.Orders.Where(o => o.UserId == userId)
//            .Select(o => new OrderDto
//            {
//                Id = o.Id,
//                TotalAmount = o.TotalAmount,
//                Status = o.Status,
//                CreatedAt = o.CreatedAt
//            }).ToListAsync();
//    }

//    // Update order status (e.g., Pending -> Shipped)
//    public async Task<OrderDto> UpdateOrderStatusAsync(int id, UpdateOrderStatusDto dto)
//    {
//        var order = await _context.Orders.FindAsync(id);
//        if (order == null) return null;

//        order.Status = dto.Status;
//        await _context.SaveChangesAsync();

//        return new OrderDto
//        {
//            Id = order.Id,
//            Status = order.Status,
//            CreatedAt = order.CreatedAt
//        };
//    }

//    // Cancel an order (if it hasn't been shipped yet)
//    public async Task<OrderDto> CancelOrderAsync(int id)
//    {
//        var order = await _context.Orders.FindAsync(id);
//        if (order == null || order.Status != OrderStatus.Pending) return null;

//        order.Status = OrderStatus.Canceled;
//        await _context.SaveChangesAsync();

//        return new OrderDto
//        {
//            Id = order.Id,
//            Status = order.Status,
//            CreatedAt = order.CreatedAt
//        };
//    }
//}
