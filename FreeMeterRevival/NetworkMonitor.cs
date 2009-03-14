
using System.Collections;
using System.Diagnostics;
namespace FreeMeterRevival
{
    // The NetworkMonitor class monitors network speed for each network adapter on the computer,
    // using classes for Performance counter in .NET library.
    public class NetworkMonitor
    {
        private ArrayList adapters;
        public ArrayList monitoredAdapters;

        public NetworkMonitor()
        {
            this.adapters = new ArrayList();
            this.monitoredAdapters = new ArrayList();
            EnumerateNetworkAdapters();
        }

        private void EnumerateNetworkAdapters()
        {
            PerformanceCounterCategory category = new PerformanceCounterCategory("Network Interface");

            foreach (string name in category.GetInstanceNames())
            {
                if (name == "MS TCP Loopback interface")
                    continue;
                NetworkAdapter adapter = new NetworkAdapter(name);
                adapter.dlCounter = new PerformanceCounter("Network Interface", "Bytes Received/sec", name);
                adapter.ulCounter = new PerformanceCounter("Network Interface", "Bytes Sent/sec", name);
                this.adapters.Add(adapter);
                adapter.init();
            }
        }

        public NetworkAdapter[] Adapters
        {
            get
            {
                return (NetworkAdapter[])this.adapters.ToArray(typeof(NetworkAdapter));
            }
        }
    }
}
