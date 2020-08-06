using Common;
using Common.SmartCardServiceInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Replicator.Connections
{
    public class DestinationProxy
    {
        IReplicateService destinationProxy;

        public DestinationProxy()
        {
            string address = "net.tcp://localhost:5003/IReplicateService";

            NetTcpBinding binding = new NetTcpBinding();
            binding.Security.Mode = SecurityMode.Transport;
            binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Windows;
            binding.Security.Transport.ProtectionLevel = System.Net.Security.ProtectionLevel.EncryptAndSign;

            ChannelFactory<IReplicateService> factoryReplicator = new ChannelFactory<IReplicateService>(binding, new EndpointAddress(address));

            destinationProxy = factoryReplicator.CreateChannel();
        }

        public void SendData(List<User> users)
        {
            destinationProxy.SendData(users);
        }
    }
}
