using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Common.SmartCardServiceInterfaces
{
    [ServiceContract]
    public interface IATMService
    {
        [OperationContract]
        bool ValidatePIN(string PIN, string username);
        [OperationContract]
        string PayIn(string username, float amount);
        [OperationContract]
        string PayOut(string username, float amount);
        [OperationContract]
        string PrintAllClients();
    }
}
