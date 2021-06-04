﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities.DataTransfer
{
	public class OrderDto
	{
		public int Id { get; set; }
		public string Email { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string FullName
		{
			get { return FirstName + " " + LastName; }
		}
		public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.Now;
		public decimal Total { get; set; }
	}
}