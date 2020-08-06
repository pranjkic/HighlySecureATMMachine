using Manager;
using System;
using System.Collections.Generic;
using System.IdentityModel.Claims;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace SecurityManager
{
    public class CustomPrincipal : IPrincipal
    {
        IIdentity identity = null;
        public CustomPrincipal(IIdentity windowsIdentity)
        {
            identity = windowsIdentity;           
        }

        public IIdentity Identity
        {
            get { return identity; }
        }

        public bool IsInRole(string permission)
        {
            X509Certificate2 clientCertificate = ((X509CertificateClaimSet)OperationContext.Current.ServiceSecurityContext.AuthorizationContext.ClaimSets[0]).X509Certificate;
            string organizationalUnit = clientCertificate.Subject.Split(',')[1].Split('=')[1];

            if (organizationalUnit == permission)
                return true;

            return false;
        }
    }
}
