using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ATM.Connections;
using Common.ATMInterfaces;
using Manager;
using SecurityManager;

namespace ATM.ATMServices
{
    public class ATMClientProvider : IATMClientService
    {
        ATMProxy aTMProxy = new ATMProxy();

        public bool ValidatePIN(string PIN, byte[] signPIN, string username, byte[] signUsername)
        {
            X509Certificate2 clientCertificate = CertManager.GetCertificateFromStorage(StoreName.My, StoreLocation.LocalMachine, username + "Sign");

            if (DigitalSignature.Verify(PIN, HashAlgorithm.SHA1, signPIN, clientCertificate) &&
                DigitalSignature.Verify(username, HashAlgorithm.SHA1, signUsername, clientCertificate)
                )
            {
                return aTMProxy.ValidatePIN(PIN, username);
            }

            return false;
        }
        public string PayIn(string username, byte[] signUsername, string amount, byte[] signAmount)
        {
            if (Thread.CurrentPrincipal.IsInRole("SmartCardUser"))
            {
                Audit.AuthorizationSuccess(username, "ATMService/PayIn");
                X509Certificate2 clientCertificate = CertManager.GetCertificateFromStorage(StoreName.My, StoreLocation.LocalMachine, username + "Sign");
                if (DigitalSignature.Verify(username, HashAlgorithm.SHA1, signUsername, clientCertificate) &&
                DigitalSignature.Verify(amount, HashAlgorithm.SHA1, signAmount, clientCertificate)
                )
                {
                    return aTMProxy.PayIn(username, Int32.Parse(amount));
                }
                return "Digital verivication faild.";
            }
            else
            {
                Audit.AuthorizationFailed(username, "ATMService/PayIn/", "No PayIn permission");
                return "User is not authorized for this action.";
            }
        }

        public string PayOut(string username, byte[] signUsername, string amount, byte[] signAmount)
        {
            if (Thread.CurrentPrincipal.IsInRole("SmartCardUser"))
            {
                Audit.AuthorizationSuccess(username, "ATMService/PayOut");
                X509Certificate2 clientCertificate = CertManager.GetCertificateFromStorage(StoreName.My, StoreLocation.LocalMachine, username + "Sign");
                if (DigitalSignature.Verify(username, HashAlgorithm.SHA1, signUsername, clientCertificate) &&
                DigitalSignature.Verify(amount, HashAlgorithm.SHA1, signAmount, clientCertificate)
                )
                {
                    return aTMProxy.PayOut(username, Int32.Parse(amount));
                }
                return "Digital verivication faild.";
            }
            else
            {
                Audit.AuthorizationFailed(username, "ATMService/PayOut/", "No PayOut permission");
                return "User is not authorized for this action.";
            }
        }

        public string PrintAllClients()
        {
            string username = Formatter.ParseName(Thread.CurrentPrincipal.Identity.Name);
            if (Thread.CurrentPrincipal.IsInRole("Manager"))
            {
                Audit.AuthorizationSuccess(username, "ATMService/PrintAllClients");
                return aTMProxy.PrintAllClients();
            }
            else
            {
                Audit.AuthorizationFailed(username, "ATMService/PrintAllClients/", "No PrintAllClientss permission");
                return "User is not authorized for this action.";
            }
        }
    }
}
