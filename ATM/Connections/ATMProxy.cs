using Common.SmartCardServiceInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace ATM.Connections
{
    public class ATMProxy
    {
        IATMService proxySCS;

        public ATMProxy()
        {
            string address = "net.tcp://localhost:4002/IATMService";
            var binding = new NetTcpBinding();
            ChannelFactory<IATMService> factoryATM = new ChannelFactory<IATMService>(binding, new EndpointAddress(address));

            proxySCS = factoryATM.CreateChannel();
        }

        public bool ValidatePIN(string PIN, string username)
        {
            return proxySCS.ValidatePIN(PIN, username);
        }

        public string PayIn(string username, float amount)
        {
            return proxySCS.PayIn(username, amount);
        }

        public string PayOut(string username, float amount)
        {
            return proxySCS.PayOut(username, amount);
        }

        public string PrintAllClients()
        {
            return proxySCS.PrintAllClients();
        }
    }
}
