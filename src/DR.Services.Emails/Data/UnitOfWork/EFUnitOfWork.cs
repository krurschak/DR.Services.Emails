using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DR.Services.Emails.Data.UnitOfWork
{
    public class EFUnitOfWork : IUnitOfWork
    {
        public EFUnitOfWork(DefaultContext context)
        {
            Context = context;
        }

        public DefaultContext Context { get; }

        public void Dispose()
        {
            if (Context != null)
            {
                Context.Dispose();
            }
        }

        public void SaveChanges()
        {
            Context.SaveChanges();
        }

        public async Task SaveChangesAsync()
        {
            await Context.SaveChangesAsync();
        }
    }
}
