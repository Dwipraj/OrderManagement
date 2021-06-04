using Application.Interfaces;
using AutoMapper;
using Core.Entities.DataTransfer;
using Core.Entities.Derived;
using Core.Entities.Table;
using DataAccess.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
	public class OrderService : IOrderService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
		private readonly ILogicalErrorMessage _errorMessage;
		private readonly ILogger<OrderService> _logger;

		public OrderService(IUnitOfWork unitOfWork, IMapper mapper, ILogicalErrorMessage errorMessage, ILogger<OrderService> logger)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
			_errorMessage = errorMessage;
			_logger = logger;
		}

		public async Task<OrderDto> CreateOrder(OrderDto orderDto)
		{
			Order order = _mapper.Map<Order>(orderDto);
			var result = await _unitOfWork.OrderRepository.Insert(order);
			_unitOfWork.Commit();

			if (result == null)
			{
				_logger.LogError($"Could not able to create {order}");
				_errorMessage.SetMessage("Could not able to create your order");
				return null;
			}

			return _mapper.Map<OrderDto>(result);
		}

		public async Task<bool> DeleteOrder(int id)
		{
			var result = await _unitOfWork.OrderRepository.Delete(id);
			_unitOfWork.Commit();

			if (!result)
			{
				_logger.LogError($"Could not able to delete order with Id {id}");
				_errorMessage.SetMessage("Could not able to delete the order.");
				return false;
			}

			return result;
		}

		public async Task<List<OrderDto>> GetAllOrders()
		{
			var result = await _unitOfWork.OrderRepository.GetAll();

			return _mapper.Map<List<Order>, List<OrderDto>>(result);
		}

		public async Task<OrderDto> GetOrderById(int id)
		{
			var result = await _unitOfWork.OrderRepository.Get(id);

			return _mapper.Map<OrderDto>(result);
		}

		public async Task<Pagination<OrderDto>> PaginateOrders(OrderPaginationDto paginationDto)
		{
			var result = await _unitOfWork.OrderRepository.GetPaginatedResult(paginationDto);
			var resultList = _mapper.Map<IReadOnlyList<Order>, IReadOnlyList<OrderDto>>(result.Data);
			return new Pagination<OrderDto>(result.PageIndex, result.PageSize, result.Count, resultList);
		}

		public async Task<bool> UpdateOrder(OrderDto orderDto)
		{
			Order order = _mapper.Map<Order>(orderDto);
			var result = await _unitOfWork.OrderRepository.Update(order);
			_unitOfWork.Commit();

			if (!result)
			{
				_logger.LogError($"Could not able to update {order}");
				_errorMessage.SetMessage("Could not able to update your order");
			}
			return result;
		}
	}
}
