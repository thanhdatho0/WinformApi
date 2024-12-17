using api.Data;
using api.Interfaces;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Repository;

public class OrderDetailRepository(ApplicationDbContext dbContext) : IOrderDetailRepository
{
    public async Task<List<OrderDetail>> GetAllAsync()
    {
        var orderDetails = 
            dbContext.OrderDetails
                .Include(o => o.Order)
                .Include(o => o.Inventory!.Product)
                .Include(o => o.Inventory!.Size)
                .Include(o => o.Inventory!.Color);
        return await orderDetails.AsNoTracking().ToListAsync();
    }

    public async Task<OrderDetail> CreateAsync(OrderDetail orderDetail)
    {
        var inventory = await dbContext.Inventories
            .Include(i => i.Product)
            .FirstOrDefaultAsync(i => i.InventoryId == orderDetail.InventoryId);
        inventory!.InStock -= orderDetail.Amount;
        inventory.Product!.InStock -= orderDetail.Amount;
        await dbContext.OrderDetails.AddAsync(orderDetail);
        await dbContext.SaveChangesAsync();
        return orderDetail;
    }
}