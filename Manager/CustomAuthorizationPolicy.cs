using Manager;
using System;
using System.Collections.Generic;
using System.IdentityModel.Claims;
using System.IdentityModel.Policy;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace SecurityManager
{
    public class CustomAuthorizationPolicy : IAuthorizationPolicy
    {        
        public CustomAuthorizationPolicy ()
        {
            Id = Guid.NewGuid().ToString();
        }

        public ClaimSet Issuer
        {
            get { return ClaimSet.System; }
        }
        public string Id
        {
            get; 
        }

        public bool Evaluate(EvaluationContext context, ref object state)
        {
            object obj;
            if (!context.Properties.TryGetValue("Identities", out obj))
                return false;

            IList<IIdentity> identities = obj as IList<IIdentity>;
            if (obj == null || identities.Count <= 0)
                return false;

            IIdentity identity = identities[0];
            try
            {
                Audit.AuthenticationSuccess("usernameParam");
            }
            catch
            {
                throw new ArgumentException(string.Format("Error while trying to write event (eventid = {0}) to event log.", (int)AuditEventTypes.UserAuthenticationSuccess));
            }
            context.Properties["Principal"] = new CustomPrincipal(identities[0]);
            return true;
        }


    }
}
