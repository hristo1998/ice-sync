using IceSync.Core.Config;
using IceSync.Core.Constants;
using IceSync.Core.Helpers;
using IceSync.Core.Services.Interfaces;
using IceSync.Core.SyncService.Interfaces;
using IceSync.Data;
using IceSync.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace IceSync.Core.SyncSerice;

public class WorkflowSyncService : BackgroundService
{
    private readonly ILogger<WorkflowSyncService> logger;
    private readonly IServiceProvider serviceProvider;

    private record WorkflowComparable(string WorkflowName, bool IsActive, string MultiExecBehavior);

    public WorkflowSyncService(
        ILogger<WorkflowSyncService> logger,
        IServiceProvider serviceProvider
        )
    {
        this.logger = logger;
        this.serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken ct)
    {
        logger.LogInformation(MessagingConstants.SyncWorkflowServiceStarted);

        while (!ct.IsCancellationRequested)
        {
            try
            {
                using var scope = serviceProvider.CreateScope();

                var ulService = scope.ServiceProvider.GetRequiredService<IUniversalLoaderService>();
                var unitOfWork = scope.ServiceProvider.GetRequiredService<IIceSyncUnitOfWork>();
                var hashStore = scope.ServiceProvider.GetRequiredService<IHashStore>();

                var apiWorkflows = await ulService.GetWorkflowsAsync();

                var newHash = HashHelper.ComputeHash(apiWorkflows);

                var lastHash = await hashStore.GetAsync(CacheKeys.Workflows);

                if (newHash == lastHash)
                {
                    logger.LogInformation("No changes detected in workflows (hash match). Skipping sync.");
                }
                else
                {
                    logger.LogInformation(MessagingConstants.SyncWorkflowStarted);
                    await SyncWorkflowsAsync(apiWorkflows, unitOfWork);


                    await hashStore.SetAsync(CacheKeys.Workflows, newHash);
                    logger.LogInformation(MessagingConstants.SyncWorkflowCompleated, newHash);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, MessagingConstants.SyncWorkflowError);
            }

            await Task.Delay(TimeSpan.FromMinutes(30), ct);
        }
    }

    private async Task SyncWorkflowsAsync(
        IList<Models.Workflow> apiWorkflows,
        IIceSyncUnitOfWork unitOfWork        
        )
    {
        var dbById = await unitOfWork.WorkflowRepository
            .All()
            .AsNoTracking()
            .ToDictionaryAsync(w => w.WorkflowId);

        // Insert or update
        foreach (var apiWf in apiWorkflows)
        {
            if (dbById.TryGetValue(apiWf.Id, out var existing))
            {
                var dbComparable = new WorkflowComparable(existing.WorkflowName, existing.IsActive, existing.MultiExecBehavior);
                var apiComparable = new WorkflowComparable(apiWf.Name, apiWf.IsActive, apiWf.MultiExecBehavior.ToString());

                if (dbComparable != apiComparable)
                {
                    (existing.WorkflowName, existing.IsActive, existing.MultiExecBehavior) = apiComparable;
                    unitOfWork.WorkflowRepository.Update(existing);
                }

                dbById.Remove(apiWf.Id);
            }
            else
            {
                // New workflow
                var newWf = new Workflow
                {
                    WorkflowId = apiWf.Id,
                    WorkflowName = apiWf.Name,
                    IsActive = apiWf.IsActive,
                    MultiExecBehavior = apiWf.MultiExecBehavior.ToString()
                };
                await unitOfWork.WorkflowRepository.AddAsync(newWf);
            }
        }

        // Delete missing ones
        foreach (var leftover in dbById.Values)
        {
            await unitOfWork.WorkflowRepository.DeleteAsync(leftover);
        }
        
        var result = unitOfWork.Save();
    }
}
