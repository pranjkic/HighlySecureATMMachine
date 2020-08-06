using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Manager
{
	public class Audit : IDisposable
	{
		
		private static EventLog customLog = null;
		const string SourceName = "Manager.Audit";
		const string LogName = "SbesProjekatTest";

		static Audit()
		{
			try
			{
                if (!EventLog.SourceExists(SourceName))
                {
                    EventLog.CreateEventSource(SourceName, LogName);
                }

                customLog = new EventLog(LogName, Environment.MachineName, SourceName);
            }
			catch (Exception e)
			{
				customLog = null;
				Console.WriteLine("Error while trying to create log handle. Error = {0}", e.Message);
			}
		}

		
		public static void AuthenticationSuccess(string userName)
		{
            string UserAuthenticationSuccess = AuditEvents.UserAuthenticationSuccess;

            if (customLog != null)
			{
                string message = string.Format(UserAuthenticationSuccess, userName);
                customLog.WriteEntry(message);
            }
			else
			{
				throw new ArgumentException(string.Format("Error while trying to write event (eventid = {0}) to event log.", (int)AuditEventTypes.UserAuthenticationSuccess));
			}
		}

		public static void AuthorizationSuccess(string userName, string serviceName)
		{
            string UserAuthorizationSuccess = AuditEvents.UserAuthorizationSuccess;
            if (customLog != null)
            {
                string message = string.Format(UserAuthorizationSuccess, userName, serviceName);
                customLog.WriteEntry(message);
            }
            else
            {
                throw new ArgumentException(string.Format("Error while trying to write event (eventid = {0}) to event log.", (int)AuditEventTypes.UserAuthorizationSuccess));
            }
        }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="userName"></param>
		/// <param name="serviceName"> should be read from the OperationContext as follows: OperationContext.Current.IncomingMessageHeaders.Action</param>
		/// <param name="reason">permission name</param>
		public static void AuthorizationFailed(string userName, string serviceName, string reason)
		{
            string UserAuthorizationFailed = AuditEvents.UserAuthorizationFailed;
            if (customLog != null)
            {
                string message = string.Format(UserAuthorizationFailed, userName, serviceName, reason);
                customLog.WriteEntry(message, EventLogEntryType.Error);
            }
            else
            {
                throw new ArgumentException(string.Format("Error while trying to write event (eventid = {0}) to event log.", (int)AuditEventTypes.UserAuthorizationFailed));
            }
        }

        public static void ReplicationSuccess()
        {
            if(customLog != null)
            {
                customLog.WriteEntry(AuditEvents.ReplicationSuccess);
            }
            else
            {
                throw new ArgumentException(string.Format("Error while trying to write event (eventid = {0}) to event log.", (int)AuditEventTypes.ReplicationSuccess));
            }
        }

        public static void ReplicationFailed()
        {
            if (customLog != null)
            {
                customLog.WriteEntry(AuditEvents.ReplicationFailed);
            }
            else
            {
                throw new ArgumentException(string.Format("Error while trying to write event (eventid = {0}) to event log.", (int)AuditEventTypes.ReplicationFailed));
            }
        }

        public static void UserChangePINSuccess(string username)
        {
            if (customLog != null)
            {
                string message = string.Format(AuditEvents.UserChangePINSuccess, username);
                customLog.WriteEntry(message, EventLogEntryType.Error);
            }
            else
            {
                throw new ArgumentException(string.Format("Error while trying to write event (eventid = {0}) to event log.", (int)AuditEventTypes.UserChangePINSuccess));
            }
        }

        public static void UserChangePINFailed(string username)
        {
            if (customLog != null)
            {
                string message = string.Format(AuditEvents.UserChangePINFailed, username);
                customLog.WriteEntry(message, EventLogEntryType.Error);
            }
            else
            {
                throw new ArgumentException(string.Format("Error while trying to write event (eventid = {0}) to event log.", (int)AuditEventTypes.UserChangePINFailed));
            }
        }

        public void Dispose()
		{
			if (customLog != null)
			{
				customLog.Dispose();
				customLog = null;
			}
		}
	}
}
