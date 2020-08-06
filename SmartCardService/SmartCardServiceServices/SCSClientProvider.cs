using Common;
using Common.SmartCardServiceInterfaces;
using Manager;
using SmartCardService.Connections;
using SmartCardService.DataBase;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SmartCardService.SmartCardServiceServices
{
    public class SCSClientProvider : ISCSClientService
    {
        public bool CertificateIssue()
        {
            string organizationUnit = FindGroup();

            string username = Formatter.ParseName(Thread.CurrentPrincipal.Identity.Name);

            string CA = "SmartCardCA";

            Random random = new Random();
            string password = (random.Next(1000, 9999)).ToString();
            Console.WriteLine("Please enter this code - " + password);

            string path = @"C:\Program Files (x86)\Windows Kits\10\bin\10.0.17763.0\x86";
            string cmd = $@"/c makecert -sv {username}.pvk -iv {CA}.pvk -n ""CN = {username}, OU={organizationUnit}"" -pe -ic {CA}.cer {username}.cer -sr localmachine -ss My -sky exchange";
            cmd = cmd.Replace(@"\", "");
            CmdHelper.Execute(path, cmd);
            CmdHelper.Execute(path, $"/c pvk2pfx.exe /pvk {username}.pvk /pi {password} /spc {username}.cer /pfx {username}.pfx");
            //CmdHelper.Execute(path, $"/c CertMgr.exe / add {username}.cer / s / r localmachine personal");
            
            CmdHelper.Execute(path, $"/c makecert -sv {username}Sign.pvk -iv {CA}.pvk -n \"CN = {username}Sign\" -pe -ic {CA}.cer {username}Sign.cer -sr localmachine -ss My -sky signature");
            CmdHelper.Execute(path, $"/c pvk2pfx.exe /pvk {username}Sign.pvk /pi {password} /spc {username}Sign.cer /pfx {username}Sign.pfx");
            //CmdHelper.Execute(path, $"/c CertMgr.exe / add {username}Sign.cer / s / r localmachine personal");

            X509Certificate2 cert = CertManager.GetCertificateFromStorage(StoreName.My, StoreLocation.LocalMachine, username);

            User user = new User
            {
                Certificate = cert,
                Name = username,
                PIN = PinToHash(username),
                Cash = 1,
                OrganizationalUnit = organizationUnit,
                TimeOfAdding = DateTime.Now,               
            };
            UserDataBase.users.Add(user);
            XMLHelper.InsertAllUserToXML(UserDataBase.users, Environment.CurrentDirectory + @"\Users.xml");
            
            return true;
        }

        public bool ChangePin(string username)
        {
            byte[] pin = PinToHash(username);
            foreach(User item in UserDataBase.users)
            {
                if(item.Name.Equals(username))
                {
                    item.PIN = pin;
                    item.TimeOfAdding = DateTime.Now;
                    Audit.UserChangePINSuccess(username);
                    XMLHelper.InsertAllUserToXML(UserDataBase.users, Environment.CurrentDirectory + @"\Users.xml");
                    return true;
                }
            }
            Audit.UserChangePINFailed(username);
            return false;
        }

        private string FindGroup()
        {
            string retVal = string.Empty;
            IIdentity identity = Thread.CurrentPrincipal.Identity;
            WindowsIdentity winIdentity = identity as WindowsIdentity;
            foreach (IdentityReference group in winIdentity.Groups)
            {
                SecurityIdentifier sid = (SecurityIdentifier)group.Translate(typeof(SecurityIdentifier));
                string name = sid.Translate(typeof(NTAccount)).ToString();
                if (name.Contains("Manager"))
                {
                    retVal = "Manager";
                    break;
                }                    
                else if (name.Contains("SmartCardUser"))
                {
                    retVal = "SmartCardUser";
                    break;
                }                    
                Console.WriteLine("{0}", name);
            }
            return retVal;
        }

        private byte[] PinToHash(string username)
        {
            Random rand = new Random();
            string pin = string.Empty;

            pin = (rand.Next(1000, 9999)).ToString();
            
            string path = AppDomain.CurrentDomain.BaseDirectory;
            path = path.Remove(path.Length - 27, 27);
            path += @"Client\bin\Debug";
            System.IO.File.WriteAllText(path + @"\" + username + "PIN.txt", pin);

            Console.WriteLine("Your PIN is generated. You can find it in Debug directory. ");

            SHA256Managed sha2 = new SHA256Managed();
            UnicodeEncoding encoding = new UnicodeEncoding();
            byte[] data = encoding.GetBytes(pin);
            byte[] hashPin = sha2.ComputeHash(data);
            return hashPin;
        }
    }
}
