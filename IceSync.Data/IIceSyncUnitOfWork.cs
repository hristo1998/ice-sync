using IceSync.Data.Entities;
using IceSync.Data.Repositories.Interfaces;

namespace IceSync.Data
{
    public interface IIceSyncUnitOfWork
    {
        IRepository<Workflow> WorkflowRepository { get; }

        int Save();

        Task<int> SaveAsync();

        void Dispose(bool disposing);

        void Dispose();
    }
}
