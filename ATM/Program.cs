using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using Common.ATMInterfaces;
using ATM.ATMServices;
using ATM.Connections;

namespace ATM
{
    class Program
    {
        static void Main(string[] args)
        {
            //Console.ReadKey();
            //Console.WriteLine("UNETO");
            ATMServiceHost atmHost = new ATMServiceHost();
            //try
            //{
            atmHost.Open();
            //}
            //catch(Exception e)
            //{
            //    Console.WriteLine("ATM HOST OPENING\n"+e.Message);
            //}
            

            Console.WriteLine("ATM is ready.");
            Console.ReadKey();

            atmHost.Close();
        }
    }
}
