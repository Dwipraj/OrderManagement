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
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
	internal abstract class BaseRepository
	{
		protected IDbTransaction Transaction { get; private set; }
		protected IDbConnection Connection { get { return Transaction.Connection; } }

		public BaseRepository(IDbTransaction transaction)
		{
			Transaction = transaction;
		}

		protected async Task<T> GetAsync<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure)
		{
			return (await Connection.QueryAsync<T>(sp, parms, transaction: Transaction, commandType: commandType)).FirstOrDefault();
		}

		protected async Task<T> GetAsync<T>(string sp, object parms, CommandType commandType = CommandType.StoredProcedure)
		{
			return (await Connection.QueryAsync<T>(sp, parms, transaction: Transaction, commandType: commandType)).FirstOrDefault();
		}

		protected async Task<List<T>> GetAllAsync<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure)
		{
			return (await Connection.QueryAsync<T>(sp, parms, transaction: Transaction, commandType: commandType)).ToList();
		}

		protected async Task<List<T>> GetAllAsync<T>(string sp, object parms, CommandType commandType = CommandType.StoredProcedure)
		{
			return (await Connection.QueryAsync<T>(sp, parms, transaction: Transaction, commandType: commandType)).ToList();
		}

		protected async Task<T> InsertAsync<T>(string sp, T parms, CommandType commandType = CommandType.StoredProcedure) where T : BaseEntity
        {
            T result;
			parms.CreatedAt = DateTimeOffset.Now;
            result = (await Connection.QueryAsync<T>(sp, parms, commandType: commandType, transaction: Transaction)).FirstOrDefault();
            return result;
        }

		protected async Task<int> UpdateAsync<T>(string sp, T parms, CommandType commandType = CommandType.StoredProcedure) where T : BaseEntity
		{
			parms.UpdatedAt = DateTimeOffset.Now;
			var result = await Connection.ExecuteAsync(sp, parms, commandType: commandType, transaction: Transaction);
			return result;
		}

		protected async Task<int> DeleteAsync<T>(string sp, T parms, CommandType commandType = CommandType.StoredProcedure) where T : BaseEntity
		{
			parms.IsDeleted = true;
			parms.DeletedAt = DateTimeOffset.Now;
			var result = await Connection.ExecuteAsync(sp, parms, commandType: commandType, transaction: Transaction);
			return result;
		}

		protected async Task<Pagination<T1>> GetPaginatedResultAsync<T1, T2>(string sp, T2 parms, string username = null, bool isCrudSp = false, CommandType commandType = CommandType.StoredProcedure) where T2 : BasePaginationDto where T1 : class
		{
			DynamicParameters parameters = new();
			parameters.Add("@Search", parms.Search, DbType.String);
			parameters.Add("@SortByColumn", parms.SortByColumn, DbType.String);
			parameters.Add("@SortByOrder", parms.SortByOrder, DbType.String);
			parameters.Add("@Offset", (parms.PageIndex - 1) * parms.PageSize, DbType.Int32);
			parameters.Add("@PageSize", parms.PageSize, DbType.Int32);

			if (isCrudSp)
			{
				parameters.Add("@ActionType", SpActionType.FetchPaginatedData.ToString(), DbType.String);
			}

			if (!string.IsNullOrEmpty(username))
			{
				parameters.Add("@Username", username, DbType.String);
			}

			var result = await Connection.QueryMultipleAsync(sp, parameters, Transaction, commandType: commandType);
			var resultList = result.Read<T1>().ToList();
			var resultCount = result.ReadFirst<int>();
			return new Pagination<T1>(parms.PageIndex, parms.PageSize, resultCount, resultList);
		}
	}
}
