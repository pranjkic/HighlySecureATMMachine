using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;

namespace Manager
{
	public enum AuditEventTypes
	{
		UserAuthenticationSuccess = 0,
		UserAuthorizationSuccess = 1,
		UserAuthorizationFailed = 2,
		ReplicationSuccess = 3,
        ReplicationFailed = 4,
        UserChangePINSuccess = 5,
        UserChangePINFailed = 6
    }

	public class AuditEvents
	{
		private static ResourceManager resourceManager = null;
		private static object resourceLock = new object();

		private static ResourceManager ResourceMgr
		{
			get
			{
				lock (resourceLock)
				{
					if (resourceManager == null)
					{
						resourceManager = new ResourceManager(typeof(AuditEventFile).FullName, Assembly.GetExecutingAssembly());
					}
					return resourceManager;
				}
			}
		}

		public static string UserAuthenticationSuccess
		{
			get
			{
				return ResourceMgr.GetString(AuditEventTypes.UserAuthenticationSuccess.ToString());
			}
		}

		public static string UserAuthorizationSuccess
		{
			get
			{
				return ResourceMgr.GetString(AuditEventTypes.UserAuthorizationSuccess.ToString());
			}
		}

		public static string UserAuthorizationFailed
		{
			get
			{
				return ResourceMgr.GetString(AuditEventTypes.UserAuthorizationFailed.ToString());
			}
		}

        public static string ReplicationSuccess
        {
			get
			{
				return ResourceMgr.GetString(AuditEventTypes.ReplicationSuccess.ToString());
			}
		}

        public static string ReplicationFailed
        {
			get
			{
				return ResourceMgr.GetString(AuditEventTypes.ReplicationFailed.ToString());
			}
		}

        public static string UserChangePINSuccess
        {
            get
            {
                return ResourceMgr.GetString(AuditEventTypes.UserChangePINSuccess.ToString());
            }
        }

        public static string UserChangePINFailed
        {
            get
            {
                return ResourceMgr.GetString(AuditEventTypes.UserChangePINFailed.ToString());
            }
        }
    }
}
