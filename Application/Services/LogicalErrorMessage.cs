﻿using Application.Interfaces;

namespace Application.Services
{
	public class LogicalErrorMessage : ILogicalErrorMessage
	{
		public string Message { get; set; } = "Some Error Occured";

		public void SetMessage(string message)
		{
			Message = message;
		}
	}
}
