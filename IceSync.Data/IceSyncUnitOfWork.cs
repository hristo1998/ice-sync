using IceSync.Data;
using IceSync.Data.Entities;
using IceSync.Data.Repositories;
using IceSync.Data.Repositories.Interfaces;

namespace ContosoUniversity.DAL
{
    public class IceSyncUnitOfWork : IIceSyncUnitOfWork, IDisposable
    {
        private AppDbContext context;
        private IRepository<Workflow> workflow;

        public IceSyncUnitOfWork(AppDbContext context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public IRepository<Workflow> WorkflowRepository
        {
            get
            {

                if (this.workflow == null)
                {
                    this.workflow = new Repository<Workflow>(context);
                }

                return workflow;
            }
        }

        public int Save()
        {
            return context.SaveChanges();
        }

        public async Task<int> SaveAsync()
        {
            return await context.SaveChangesAsync();
        }

        private bool disposed = false;

        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}