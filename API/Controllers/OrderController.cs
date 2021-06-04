using API.Errors;
using API.Middleware;
using Application.Interfaces;
using Core.Entities.DataTransfer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class OrderController : ControllerBase
	{
		private readonly IOrderService _orderService;
		private readonly ILogicalErrorMessage _errorMessage;

		public OrderController(IOrderService orderService, ILogicalErrorMessage errorMessage)
		{
			_orderService = orderService;
			_errorMessage = errorMessage;
		}

		[Authorize]
		[ServiceFilter(typeof(AdminOrOwnerFilter))]
		[HttpGet("[action]/{id}")]
		public async Task<ActionResult<OrderDto>> GetOrder(int id)
		{
			var result = await _orderService.GetOrderById(id);

			if (result == null)
			{
				return NotFound(new ApiResponse(404, _errorMessage.Message));
			}

			return Ok(result);
		}

		[Authorize]
		[ServiceFilter(typeof(AdminOrOwnerFilter))]
		[HttpPost("[action]")]
		public async Task<ActionResult<OrderDto>> PaginateOrder([FromBody] OrderPaginationDto paginationDto)
		{
			var result = await _orderService.PaginateOrders(paginationDto);

			return Ok(result);
		}

		[Authorize]
		[HttpPost("[action]")]
		public async Task<ActionResult<OrderDto>> CreateOrder([FromBody] OrderDto order)
		{
			var result = await _orderService.CreateOrder(order);

			if (result == null)
			{
				return BadRequest(new ApiResponse(400, _errorMessage.Message));
			}

			return Ok(result);
		}

		[Authorize]
		[ServiceFilter(typeof(OwnerFilter))]
		[HttpPut("[action]/{id}")]
		public async Task<ActionResult> UpdateOrder(int id, [FromBody] OrderDto order)
		{
			if (id != order.Id)
			{
				return BadRequest(new ApiResponse(400, "Order id does not match."));
			}

			var result = await _orderService.UpdateOrder(order);

			if (!result)
			{
				return BadRequest(new ApiResponse(400, _errorMessage.Message));
			}

			return Ok();
		}

		[Authorize]
		[ServiceFilter(typeof(OwnerFilter))]
		[HttpDelete("[action]/{id}")]
		public async Task<ActionResult> DeleteOrder(int id)
		{
			var result = await _orderService.DeleteOrder(id);

			if (!result)
			{
				return BadRequest(new ApiResponse(400, _errorMessage.Message));
			}

			return Ok();
		}
	}
}
