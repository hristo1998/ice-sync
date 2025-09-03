using IceSync.Core.Constants;
using IceSync.Core.Services.Interfaces;
using IceSync.Data;
using IceSync.Data.Entities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace IceSync.Core.SyncSerice;

public class WorkflowSyncService : BackgroundService
{
    private readonly ILogger<WorkflowSyncService> logger;
    private readonly IServiceProvider serviceProvider;

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

                await SyncWorkflowsAsync(ulService, unitOfWork, ct);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, MessagingConstants.SyncWorkflowError);
            }

            await Task.Delay(TimeSpan.FromMinutes(30), ct);
        }
    }

    private async Task SyncWorkflowsAsync(
        IUniversalLoaderService ulService,
        IIceSyncUnitOfWork unitOfWork,
        CancellationToken ct
        )
    {
        logger.LogInformation(MessagingConstants.SyncWorkflowStarted);

        var apiWorkflows = await ulService.GetWorkflowsAsync();
        var dbWorkflows = await unitOfWork.WorkflowRepository.GetAsync();

        var dbById = dbWorkflows.ToDictionary(w => w.WorkflowId);

        // Insert or update
        foreach (var apiWf in apiWorkflows)
        {
            if (dbById.TryGetValue(apiWf.Id, out var existing))
            {
                if (existing.WorkflowName != apiWf.Name ||
                    existing.IsActive != apiWf.IsActive ||
                    existing.MultiExecBehavior != apiWf.MultiExecBehavior.ToString())
                {
                    existing.WorkflowName = apiWf.Name;
                    existing.IsActive = apiWf.IsActive;
                    existing.MultiExecBehavior = apiWf.MultiExecBehavior.ToString();
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
 
        logger.LogInformation(MessagingConstants.SyncWorkflowCompleated);
    }
}
