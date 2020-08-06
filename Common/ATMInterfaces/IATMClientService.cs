using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Common.ATMInterfaces
{
    [ServiceContract]
    public interface IATMClientService
    {
        [OperationContract]
        bool ValidatePIN(string PIN, byte[] signPIN, string username, byte[] signUsername);
        [OperationContract]
        string PayIn(string username, byte[] signUsername, string amount, byte[] signAmount);
        [OperationContract]
        string PayOut(string username, byte[] signUsername, string amount, byte[] signAmount);
        [OperationContract]
        string PrintAllClients();
    }
}
