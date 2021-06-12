namespace DarkRift.Nat
{
    /// <summary>
    /// The state of the remote router.
    /// </summary>
    public enum ForwardingStatus
    {
        /// <summary>
        /// Indicates forwarding has not yet started.
        /// </summary>
        NotStarted,

        /// <summary>
        /// Indicates forwarding is currently in progress.
        /// </summary>
        InProgress,
    
        /// <summary>
        /// Indicates forwarding has completed for all listeners successfully.
        /// </summary>
        Succeeded,
    
        /// <summary>
        /// Indicates one of more of the forwards has failed to apply. Other forwardings may still be in progress.
        /// </summary>
        Failed,

        /// <summary>
        /// Indicates forwarding is unavailable as no network devices support it.
        /// </summary>
        Unavailable
    }
}