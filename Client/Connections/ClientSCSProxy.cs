using Common.SmartCardServiceInterfaces;
using Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Principal;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Client.Connections
{
    public class ClientSCSProxy
    {
        ISCSClientService proxySCS;

        public ClientSCSProxy()
        {

            string address = "net.tcp://localhost:4001/ISCSClientService";

            NetTcpBinding binding = new NetTcpBinding();
            binding.Security.Mode = SecurityMode.Transport;
            binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Windows;
            binding.Security.Transport.ProtectionLevel = System.Net.Security.ProtectionLevel.EncryptAndSign;

            ChannelFactory<ISCSClientService> factorySCS = new ChannelFactory<ISCSClientService>(binding, new EndpointAddress(address));

            proxySCS = factorySCS.CreateChannel();
        }

        public bool CertificateIssue()
        {
            return proxySCS.CertificateIssue();
        }

        public bool ChangePin(string username)
        {
            return proxySCS.ChangePin(username);
        }
    }
}
