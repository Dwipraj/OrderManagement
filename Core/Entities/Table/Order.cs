using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities.Table
{
	public class Order : BaseEntity
	{
		public Order()
		{
		}

		public Order(Order order)
		{
			Id = order.Id;
			Email = order.Email;
			FirstName = order.FirstName;
			LastName = order.LastName;
			OrderDate = order.OrderDate;
			Total = order.Total;
			IsDeleted = order.IsDeleted;
			CreatedAt = order.CreatedAt;
			UpdatedAt = order.UpdatedAt;
			DeletedAt = order.DeletedAt;
		}

		public string Email { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.Now;
		public decimal Total { get; set; }

		public override string ToString()
		{
			return $"Order - OrderId: {Id}, Email: {Email}, Name: {FirstName + " " + LastName}, OrderDate: {OrderDate}, Total: {Total}, Deleted: {IsDeleted}";
		}
	}
}
