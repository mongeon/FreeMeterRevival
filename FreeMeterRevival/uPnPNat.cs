using System;
using System.Collections.Generic;
using System.Text;
using NatTraversal.Interop;
using System.Collections;
using System.Net;

namespace FreeMeterRevival
{
    public class _UPnPNat
    {
        private UPnPNAT upnp;

        public _UPnPNat()
        {
            try
            {
                UPnPNAT nat = new UPnPNAT();
                if (nat.NATEventManager != null && nat.StaticPortMappingCollection != null)
                    upnp = nat;
            }
            catch { }

            if (upnp == null) // No configurable UPNP NAT is available.
                throw new NotSupportedException();
        }
        public PortMappingInfo[] PortMappings
        {
            get
            {
                ArrayList portMappings = new ArrayList();

                // Enumerates the ports without using the foreach statement (causes the interop to fail).
                int count = upnp.StaticPortMappingCollection.Count;
                IEnumerator enumerator = upnp.StaticPortMappingCollection.GetEnumerator();
                enumerator.Reset();

                for (int i = 0; i <= count; i++)
                {
                    IStaticPortMapping mapping = null;
                    try
                    {
                        if (enumerator.MoveNext())
                            mapping = (IStaticPortMapping)enumerator.Current;
                    }
                    catch { }

                    if (mapping != null)
                        portMappings.Add(new PortMappingInfo(mapping.Description, mapping.Protocol.ToUpper(), mapping.InternalClient, mapping.InternalPort, IPAddress.Parse(mapping.ExternalIPAddress), mapping.ExternalPort, mapping.Enabled));
                }

                // copies the ArrayList to an array of PortMappingInfo.
                PortMappingInfo[] portMappingInfos = new PortMappingInfo[portMappings.Count];
                portMappings.CopyTo(portMappingInfos);

                return portMappingInfos;
            }
        }

        public void AddPortMapping(PortMappingInfo portMapping)
        {
            upnp.StaticPortMappingCollection.Add(portMapping.ExternalPort, portMapping.Protocol, portMapping.InternalPort, portMapping.InternalHostName, portMapping.Enabled, portMapping.Description);
        }

        public void RemovePortMapping(PortMappingInfo portMapping)
        {
            upnp.StaticPortMappingCollection.Remove(portMapping.ExternalPort, portMapping.Protocol);
        }
    }
}
