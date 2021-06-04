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
	public interface IBaseRepository
	{
		T Get<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.Text);
		List<T> GetAll<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure);
		T Insert<T>(string sp, T parms, CommandType commandType = CommandType.StoredProcedure) where T : BaseEntity;
		T Update<T>(string sp, T parms, CommandType commandType = CommandType.StoredProcedure) where T : BaseEntity;
	}
}
