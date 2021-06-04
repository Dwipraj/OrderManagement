using Core.Entities.DataTransfer;
using Core.Entities.Derived;
using Core.Entities.Table;
using Core.Enums;
using Dapper;
using DataAccess.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
	internal class OrderRepository : BaseRepository, IOrderRepository
	{
		private const string CRUD_SP_NAME = "SP_ORDER";
		private readonly string _username;

		public OrderRepository(IDbTransaction transaction, string username = null) : base(transaction)
		{
			_username = username;
		}

		public async Task<bool> Delete(int id)
		{
			OrderSp orderSp = new(id, SpActionType.DeleteData, _username);
			int result = await DeleteAsync(CRUD_SP_NAME, orderSp);

			if (result != 1)
			{
				throw new Exception($"Something went wrong {result} row(s) affected.");
			}

			return true;
		}

		public async Task<Order> Get(int id)
		{
			OrderSp orderSp = new(id, SpActionType.FetchData, _username);
			return await GetAsync<Order>(CRUD_SP_NAME, orderSp);
		}

		public async Task<List<Order>> GetAll()
		{
			OrderSp orderSp = new(SpActionType.FetchAllData, _username);
			return await GetAllAsync<Order>(CRUD_SP_NAME, orderSp);
		}

		public async Task<Pagination<Order>> GetPaginatedResult(OrderPaginationDto paginationDto)
		{
			return await GetPaginatedResultAsync<Order, OrderPaginationDto>(CRUD_SP_NAME, paginationDto, username: _username, isCrudSp: true);
		}

		public async Task<Order> Insert(Order order)
		{
			OrderSp orderSp = new(order, SpActionType.SaveData, _username);
			return await InsertAsync<Order>(CRUD_SP_NAME, orderSp);
		}

		public async Task<bool> Update(Order order)
		{
			OrderSp orderSp = new(order, SpActionType.SaveData, _username);
			int result = await UpdateAsync(CRUD_SP_NAME, orderSp);

			if (result != 1)
			{
				throw new Exception($"Something went wrong {result} row(s) affected.");
			}

			return true;
		}
	}
}
