using DataAccess.Interfaces;
using DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
	public class UnitOfWork : IUnitOfWork
	{
		private IDbConnection _connection;
		private IDbTransaction _transaction;
        private IOrderRepository _orderRepository;
        private string _username;

        public UnitOfWork(string connectionString)
		{
			_connection = new SqlConnection(connectionString);
			_connection.Open();
			_transaction = _connection.BeginTransaction();
		}

		public IOrderRepository OrderRepository => _orderRepository ??= new OrderRepository(_transaction, _username);

		public void Commit()
        {
            try
            {
                _transaction.Commit();
            }
            catch
            {
                _transaction.Rollback();
                throw;
            }
            finally
            {
                _transaction.Dispose();
                _transaction = _connection.BeginTransaction();
                ResetRepositories();
            }
        }

        public void Dispose()
        {
            if (_transaction != null)
            {
                _transaction.Dispose();
                _transaction = null;
            }
            if (_connection != null)
            {
				if (_connection.State == ConnectionState.Open)
				{
                    _connection.Close();
				}
                _connection.Dispose();
                _connection = null;
            }

            GC.SuppressFinalize(this);
        }

		public void SetUsername(string username)
		{
			_username = username;
		}

		private void ResetRepositories()
        {
            _orderRepository = null;
        }
    }
}
