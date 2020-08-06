using Common;
using Common.SmartCardServiceInterfaces;
using Manager;
using SmartCardService.DataBase;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace SmartCardService.SmartCardServiceServices
{
    public class SCSATMProvider : IATMService
    {
        public bool ValidatePIN(string PIN, string username)
        {
            UnicodeEncoding encoding = new UnicodeEncoding();
            byte[] data = encoding.GetBytes(PIN);
            SHA256Managed sha1 = new SHA256Managed();
            byte[] hashPIN = sha1.ComputeHash(data);

            foreach (User user in UserDataBase.users)
            {
                if (user.Name == username)
                {
                    string b1 = string.Empty;
                    string b2 = string.Empty;
                    foreach (byte b in user.PIN)
                        b1 += b.ToString();
                    foreach (byte b in hashPIN)
                        b2 += b.ToString();
                    Console.WriteLine("\n" + b1);
                    Console.WriteLine(b2 + "\n");
                    if (string.Compare(b1, b2) == 0)
                        return true;
                    else
                        return false;
                }
            }
                 
            return false;            
        }

        public string PayIn(string username, float amount)
        {
            foreach(User user in UserDataBase.users)
            {
                if (user.Name == username)
                {
                    user.Cash += amount;
                    XMLHelper.InsertAllUserToXML(UserDataBase.users, Environment.CurrentDirectory + @"\Users.xml");
                    return "Successfull transaction, your current amount is " + user.Cash.ToString();
                }                    
            }
            return "Transaction failed.";
        }

        public string PayOut(string username, float amount)
        {
            double currentAmount = 0;
            foreach (User user in UserDataBase.users)
            {
                if (user.Name == username)
                {
                    if(user.Cash >= amount)
                    {
                        user.Cash -= amount;
                        XMLHelper.InsertAllUserToXML(UserDataBase.users, Environment.CurrentDirectory + @"\Users.xml");
                        return "Successfull transaction, your current amount is " + user.Cash.ToString();
                    }
                    currentAmount = user.Cash;
                    break;
                }
            }
            return "Your available amount is " + currentAmount.ToString() + ", you cannot PayOut " + amount.ToString();
        }

        public string PrintAllClients()
        {
            string retVal = string.Empty;
            foreach(User user in UserDataBase.users)
            {
                retVal += user.Name + " - " + user.OrganizationalUnit + "\n";
            }
            return retVal;
        }
    }
}
