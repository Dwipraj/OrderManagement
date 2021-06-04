using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities.Table
{
	/// <summary>
	/// This is useful as we are using repository pattern, we can modify the base properties based on action in BaseRepository class
	/// </summary>
	public class BaseEntity
	{
		public int Id { get; set; }
		public bool IsDeleted { get; set; }
		public DateTimeOffset? CreatedAt { get; set; }
		public DateTimeOffset? UpdatedAt { get; set; }
		public DateTimeOffset? DeletedAt { get; set; }
	}
}
