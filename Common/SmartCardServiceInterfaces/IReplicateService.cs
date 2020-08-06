using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Common.SmartCardServiceInterfaces
{
    [ServiceContract]
    public interface IReplicateService
    {
        [OperationContract]
        void SendData(List<User> users);
        [OperationContract]
        List<User> ReciveData();
    }
}
