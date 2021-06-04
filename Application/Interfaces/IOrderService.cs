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
		Task<bool> UpdateOrder(OrderDto orderDto);
		Task<Pagination<OrderDto>> PaginateOrders(OrderPaginationDto paginationDto);
	}
}
