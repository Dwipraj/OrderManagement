using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
	public interface ILogicalErrorMessage
	{
		string Message { get; }
		void SetMessage(string message);
	}
}
