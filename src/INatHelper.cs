using System.Net;

namespace DarkRift.Nat
{
    /// <summary>
    /// Provides automatic port forwarding via UPNP and PMP.
    /// </summary>
    public interface INatHelper
    {
        /// <summary>
        /// The external IP address of the server.
        /// </summary>
        /// <value>The external IP address of the server.</value>
        IPAddress ExternalIPAddress { get; }

        /// <summary>
        /// The current state of the remote router.
        /// </summary>
        /// <value>The current state of the remote router</value>
        ForwardingStatus ForwardingStatus { get; }
    }
}
