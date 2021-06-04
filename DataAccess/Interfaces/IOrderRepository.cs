using Core.Entities.DataTransfer;
using Core.Entities.Derived;
using Core.Entities.Table;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Interfaces
{
	public interface IOrderRepository
	{
		Task<Order> Get(int id);
		Task<List<Order>> GetAll();
		Task<Pagination<Order>> GetPaginatedResult(OrderPaginationDto paginationDto);
		Task<Order> Insert(Order order);
		Task<bool> Update(Order order);
		Task<bool> Delete(int id);
	}
}
