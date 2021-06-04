using Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities.DataTransfer
{
	public class OrderPaginationDto : BasePaginationDto
	{
		public OrderSortByColumn SortByColumnType { get; set; } = OrderSortByColumn.Id;

		public override string SortByColumn => SortByColumnType.ToString();
	}
}
