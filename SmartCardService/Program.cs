using Common;
using Common.SmartCardServiceInterfaces;
using SmartCardService.Connections;
using SmartCardService.DataBase;
using SmartCardService.SmartCardServiceServices;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SmartCardService
{
    class Program
    {
        static void Main(string[] args)
        {
            SmartCardReplicatorServiceHost smartCardReplicatorServiceHost = new SmartCardReplicatorServiceHost();
            UserDataBase.users = XMLHelper.ReadAllUserFromXML(Environment.CurrentDirectory + @"\Users.xml");

            SmartCardATMServiceHost smartCardATMServiceHost = new SmartCardATMServiceHost();
            SmartCardClientServiceHost smartCardClientServiceHost = new SmartCardClientServiceHost();

            smartCardReplicatorServiceHost.Open();
            smartCardATMServiceHost.Open();
            smartCardClientServiceHost.Open();
            Console.WriteLine("SmartCardService is ready.");
            Console.ReadKey();
            smartCardClientServiceHost.Close();
            smartCardATMServiceHost.Close();
            smartCardReplicatorServiceHost.Close();
        }
    }
}
