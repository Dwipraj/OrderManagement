using Core.Entities.Table;
using Core.Enums;
using Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities.Derived
{
	public class OrderSp : Order, IBaseCrudSp
	{
		public OrderSp(Order order, SpActionType actionType, string username = null) : base(order)
		{
			ActionType = actionType.ToString();
			Username = username;
		}

		public OrderSp(int id, SpActionType actionType, string username = null)
		{
			Id = id;
			ActionType = actionType.ToString();
			Username = username;
		}

		public OrderSp(SpActionType actionType, string username = null)
		{
			ActionType = actionType.ToString();
			Username = username;
		}

		public OrderSp()
		{
		}

		public string ActionType { get; private set; }
		public string Username { get; private set; }
	}
}
