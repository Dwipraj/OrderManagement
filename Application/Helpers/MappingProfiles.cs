using AutoMapper;
using Core.Entities.DataTransfer;
using Core.Entities.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Helpers
{
	public class MappingProfiles : Profile
	{
		public MappingProfiles()
		{
			CreateMap<Order, OrderDto>().ReverseMap();
		}
	}
}
