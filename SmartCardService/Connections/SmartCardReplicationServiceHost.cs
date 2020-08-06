using Common;
using Common.SmartCardServiceInterfaces;
using SmartCardService.SmartCardServiceServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace SmartCardService.Connections
{
    public class SmartCardReplicatorServiceHost
    {
        ServiceHost serviceHost;

        public SmartCardReplicatorServiceHost()
        {
            string addess = ServiceHostHelper.GetBaseAddresses(ServiceHostEnum.REPLICATOR_HOST);

            NetTcpBinding binding = new NetTcpBinding();
            binding.Security.Mode = SecurityMode.Transport;
            binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Windows;
            binding.Security.Transport.ProtectionLevel = System.Net.Security.ProtectionLevel.EncryptAndSign;

            serviceHost = new ServiceHost(typeof(SCSReplicatorProvider));
            serviceHost.AddServiceEndpoint(typeof(IReplicateService), binding, new Uri(addess));
        }
        public void Open()
        {
            serviceHost.Open();
        }
        public void Close()
        {
            serviceHost.Close();
        }
    }
}
