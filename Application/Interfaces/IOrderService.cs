using Core.Entities.DataTransfer;
using Core.Entities.Derived;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
	public interface IOrderService
	{
		Task<OrderDto> CreateOrder(OrderDto orderDto);
		Task<OrderDto> GetOrderById(int id);
		Task<List<OrderDto>> GetAllOrders();
		Task<bool> DeleteOrder(int id);
		Task<bool> UpdateOrder(OrderDto orderDto);
		Task<Pagination<OrderDto>> PaginateOrders(OrderPaginationDto paginationDto);
	}
}
