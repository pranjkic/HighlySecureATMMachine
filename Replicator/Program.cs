using Common;
using Manager;
using Replicator.Connections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Replicator
{
    class Program
    {
        static void Main(string[] args)
        {
            SourceProxy sourceProxy = new SourceProxy();
            DestinationProxy destinationProxy = new DestinationProxy();

            List<User> users = new List<User>();
            while (true)
            {
                try
                {
                    users = sourceProxy.ReciveData();
                    destinationProxy.SendData(users);
                    Audit.ReplicationSuccess();
                }
                catch (Exception e)
                {
                    Audit.ReplicationFailed();
                }
                Thread.Sleep(5000);
            }
        }
    }
}
