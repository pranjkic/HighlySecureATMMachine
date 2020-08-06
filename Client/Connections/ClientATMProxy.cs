using Common.ATMInterfaces;
using Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Client.Connections
{
    public class ClientATMProxy
    {
        IATMClientService proxyATM;

        public ClientATMProxy()
        {
            var binding = new NetTcpBinding();
            //binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Certificate;

            binding.Security.Mode = SecurityMode.Transport;
            binding.Security.Transport.ProtectionLevel = System.Net.Security.ProtectionLevel.EncryptAndSign;
            binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Certificate;

            X509Certificate2 srvCert = CertManager.GetCertificateFromStorage(StoreName.TrustedPeople, StoreLocation.LocalMachine, /**/"ATMService");
            EndpointAddress address = new EndpointAddress(new Uri("net.tcp://localhost:6004/IATMClientService"),
                                      new X509CertificateEndpointIdentity(srvCert));

            ChannelFactory<IATMClientService> factoryATM = new ChannelFactory<IATMClientService>(binding, address);
            factoryATM.Credentials.ServiceCertificate.Authentication.CertificateValidationMode = System.ServiceModel.Security.X509CertificateValidationMode.ChainTrust;
            factoryATM.Credentials.ServiceCertificate.Authentication.RevocationMode = X509RevocationMode.NoCheck;

            /// Set appropriate client's certificate on the channel. Use CertManager class to obtain the certificate based on the "cltCertCN"
            factoryATM.Credentials.ClientCertificate.Certificate = CertManager.GetCertificateFromStorage(StoreName.My, StoreLocation.LocalMachine, Formatter.ParseName(WindowsIdentity.GetCurrent().Name));

            proxyATM = factoryATM.CreateChannel();
        }
        public bool ValidatePIN(string PIN, byte[] signPIN,string username, byte[] signUsername)
        {
            return proxyATM.ValidatePIN(PIN, signPIN, username, signUsername);
        }
        public string PayIn(string username, byte[] signUsername, string amount, byte[] signAmount)
        {
            return proxyATM.PayIn(username, signUsername, amount, signAmount);
        }
        public string PayOut(string username, byte[] signUsername, string amount, byte[] signAmount)
        {
            return proxyATM.PayOut(username, signUsername, amount, signAmount);
        }
        public string PrintAllClients()
        {
            return proxyATM.PrintAllClients();
        }
    }
}
