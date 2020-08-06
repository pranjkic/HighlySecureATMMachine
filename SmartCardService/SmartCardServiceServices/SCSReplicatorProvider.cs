using Common;
using Common.SmartCardServiceInterfaces;
using SmartCardService.DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCardService.SmartCardServiceServices
{
    public class SCSReplicatorProvider : IReplicateService
    {
        public List<User> ReciveData()
        {
            return UserDataBase.users;
            
        }

        public void SendData(List<User> users)
        {
            XMLHelper.InsertAllUserToXML(users, Environment.CurrentDirectory + @"\ReplicatedUsers.xml");
            UserDataBase.users.Clear();
            foreach (User user in users)
            {
                UserDataBase.users.Add(user);             
            }
        }
    }
}
