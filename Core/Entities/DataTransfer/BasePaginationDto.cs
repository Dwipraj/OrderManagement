using Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities.DataTransfer
{
	/// <summary>
	/// Base properties of pagination. Child classes can use additional property SotyBy of type Enum.
	/// </summary>
	public abstract class BasePaginationDto
	{
		private const int MaxPageSize = 50;
		public int PageIndex { get; set; } = 1;

		private int _pageSize = 10;
		public int PageSize
		{
			get => _pageSize;
			set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
		}

		private string _search;
		public string Search
		{
			get => _search;
			set => _search = value.ToLower();
		}

		/// <summary>
		/// This value must be set using an enum by child classes as this will be more suggestive in SWAGGER documentation.
		/// As different result set will need different column names.
		/// So it's best if we let child classes handle the SortBy Column names.
		/// </summary>
		public abstract string SortByColumn { get; }
		public SortByOrderType SortByOrderType { get; set; } = SortByOrderType.ASC;
		public string SortByOrder { get => SortByOrderType.ToString(); }
	}
}
