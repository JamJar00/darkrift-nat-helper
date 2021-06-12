using System.Threading.Tasks;
using System;
using System.Net;
using DarkRift.Server;
using Open.Nat;
using System.Linq;

namespace DarkRift.Nat
{
    /// <summary>
    /// Provides automatic port forwarding via UPNP and PMP.
    /// </summary>
    internal class NatHelper : Plugin, INatHelper
    {
        /// <summary>
        /// The version number of your plugin. Increasing this will trigger a plugin upgrade in DarkRift Pro.
        /// </summary>
        public override Version Version => new Version(1, 0, 0);

        /// <summary>
        /// Flag to DarkRift stating if this plugin can handle events from multiple threads. If you're unsure, leave this false!
        /// </summary>
        public override bool ThreadSafe => false;

        /// <inheritdoc />
        public IPAddress ExternalIPAddress { get; private set; }

        /// <inheritdoc />
        public ForwardingStatus ForwardingStatus { get; private set; } = ForwardingStatus.NotStarted;

        /// <summary>
        /// Constructor. Called as the plugin is loaded.
        /// </summary>
        public NatHelper(PluginLoadData pluginLoadData) : base(pluginLoadData)
        {

        }

        protected override void Loaded(LoadedEventArgs args)
        {
            Setup();
        }

        private async void Setup()
        {
            ForwardingStatus = ForwardingStatus.InProgress;

            NatDiscoverer discoverer = new NatDiscoverer();
            NatDevice device;
            try
            {
                device = await discoverer.DiscoverDeviceAsync();
            }
            catch (NatDeviceNotFoundException e)
            {
                Logger.Warning("No UPNP nor PMP compatible device found to port forward.", e);
                ForwardingStatus = ForwardingStatus.Unavailable;
                return;
            }

            await FetchIPAddress(device);

            await Task.WhenAll(NetworkListenerManager.GetNetworkListeners().Select(l => PortForwardFor(device, l)));

            if (ForwardingStatus != ForwardingStatus.Failed)
                ForwardingStatus = ForwardingStatus.Succeeded;
        }

        private async Task FetchIPAddress(NatDevice device)
        {
            ExternalIPAddress = await device.GetExternalIPAsync();
        }

        private async Task PortForwardFor(NatDevice device, NetworkListener listener)
        {
            try
            {
                Task tcpForward = device.CreatePortMapAsync(new Mapping(Protocol.Tcp, listener.Port, listener.Port, "DarkRift TCP"));
                Task udpForward = device.CreatePortMapAsync(new Mapping(Protocol.Udp, listener.Port, listener.Port, "DarkRift UDP"));

                await tcpForward;
                await udpForward;

                Logger.Info($"Successfully forwarded ports for listener '{listener.Name}'.");
            }
            catch (MappingException e)
            {
                Logger.Error($"Failed to forward ports for listener '{listener.Name}' port {listener.Port}.", e);
                ForwardingStatus = ForwardingStatus.Failed;
            }
        }
    }
}
