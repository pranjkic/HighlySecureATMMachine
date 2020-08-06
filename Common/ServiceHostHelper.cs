using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Common
{
    public class ServiceHostHelper
    {
        public static string GetBaseAddresses(ServiceHostEnum serviceHostType)
        {
            List<string> retValues = new List<string>();

            XmlDocument xmlDoc = new XmlDocument();

            xmlDoc.Load(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);

            xmlDoc.SelectNodes("/configuration/system.serviceModel/services/service/host/baseAddresses/add")
                .Cast<XmlNode>().ToList()
                .ForEach(o => retValues.Add(o.Attributes["baseAddress"].Value));

            return retValues[(int)serviceHostType];
        }
    }

    public enum ServiceHostEnum : int
    {
        CLIENT_HOST,
        ATM_HOST,
        REPLICATOR_HOST
    }
}
