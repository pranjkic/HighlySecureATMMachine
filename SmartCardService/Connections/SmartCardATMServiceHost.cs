using Common;
using Common.SmartCardServiceInterfaces;
using SmartCardService.SmartCardServiceServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace SmartCardService.Connections
{
    public class SmartCardATMServiceHost
    {
        ServiceHost serviceHost;

        public SmartCardATMServiceHost()
        {
            string addess = ServiceHostHelper.GetBaseAddresses(ServiceHostEnum.ATM_HOST);
            var binding = new NetTcpBinding();
            serviceHost = new ServiceHost(typeof(SCSATMProvider));
            serviceHost.AddServiceEndpoint(typeof(IATMService), binding, new Uri(addess));
           
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
