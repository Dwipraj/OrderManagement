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
        /// <summary>
        /// As this service is registered as a scoped service it will keep the connection open until.
        /// </summary>
        /// <param name="connectionString">Database Connection String</param>
        public UnitOfWork(string connectionString)
		{
			_connection = new SqlConnection(connectionString);
			_connection.Open();
			_transaction = _connection.BeginTransaction();
		}

        /// <summary>
        /// New Repositories will be added like this fashion. Don't forget to SET newly added repositories' value to null in ResetRepositories() method.
        /// </summary>
		public IOrderRepository OrderRepository => _orderRepository ??= new OrderRepository(_transaction, _username);

        /// <summary>
        /// Always remember to call commit after a Command(Create/Update/Delete), otherwise data will not be saved.
        /// </summary>
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

        /// <summary>
        /// DO NOT CALL Commit() FROM Dispose()
        /// Yes, if you call Commit() here this will make your code much cleaner.
        /// But this could also lead to unexpected bugs.
        /// </summary>
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

        /// <summary>
        /// This username will persist in current request context and could be user for accessibility verification by repositories.
        /// </summary>
        /// <param name="username">Username of the user against whom data access needs to be verified</param>
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
