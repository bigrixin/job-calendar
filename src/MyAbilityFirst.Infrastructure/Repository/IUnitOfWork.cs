using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyAbilityFirst.Infrastructure
{
	public interface IUnitOfWork
	{
		void Save();
		Task SaveAsync();
	}
}