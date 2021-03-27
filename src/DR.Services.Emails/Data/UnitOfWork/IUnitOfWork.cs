using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DR.Services.Emails.Data.UnitOfWork
{
	public interface IUnitOfWork : IDisposable
	{
		void SaveChanges();
		Task SaveChangesAsync();
	}
}
