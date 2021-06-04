using System;

namespace DataAccess.Interfaces
{
	public interface IUnitOfWork : IDisposable
	{
		IOrderRepository OrderRepository { get; }

		void Commit();
		void SetUsername(string username);
	}
}
