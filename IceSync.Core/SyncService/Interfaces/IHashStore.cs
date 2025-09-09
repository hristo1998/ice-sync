namespace IceSync.Core.SyncService.Interfaces
{
    public interface IHashStore
    {
        Task<string?> GetAsync(string key);
        Task SetAsync(string key, string hash);
    }
}
