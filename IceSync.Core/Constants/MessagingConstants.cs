namespace IceSync.Core.Constants
{
    internal static class MessagingConstants
    {
        public static string SyncWorkflowServiceStarted = "WorkflowSyncService started";
        public static string SyncWorkflowError = "Error while syncing workflows";
        public static string SyncWorkflowStarted = "Starting workflow sync...";
        public static string SyncWorkflowCompleated = "Stored new workflows hash: {Hash}";
        public const string AuthenticationFailedErrorMessage = "Authentication failed: no access token acquired.";
    }
}
