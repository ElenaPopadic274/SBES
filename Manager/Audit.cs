using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manager
{
    public class Audit : IDisposable
    {

        private static EventLog customLog = null;
        const string SourceName = "Manager.Audit";
        const string LogName = "MySecTest";

        static Audit()
        {
            try
            {
                if (!EventLog.SourceExists(SourceName))
                {
                    EventLog.CreateEventSource(SourceName, LogName);
                }
                customLog = new EventLog(LogName,
                    Environment.MachineName, SourceName);
            }
            catch (Exception e)
            {
                customLog = null;
                Console.WriteLine("Error while trying to create log handle. Error = {0}", e.Message);
            }
        }

        #region AAA
        public static void AuthenticationSuccess(string userName)
        {
            if (customLog != null)
            {
                string UserAuthenticationSuccess =
                    AuditEvents.AuthenticationSuccess;
                string message = String.Format(UserAuthenticationSuccess,
                    userName);
                customLog.WriteEntry(message);
            }
            else
            {
                throw new ArgumentException(string.Format("Error while trying to write event (eventid = {0}) to event log.",
                    (int)AuditEventTypes.AuthenticationSuccess));
            }
        }

        public static void AuthorizationSuccess(string userName, string serviceName)
        {
            //TO DO
            if (customLog != null)
            {
                string AuthorizationSuccess =
                    AuditEvents.AuthorizationSuccess;
                string message = String.Format(AuthorizationSuccess,
                    userName, serviceName);
                customLog.WriteEntry(message);
            }
            else
            {
                throw new ArgumentException(string.Format("Error while trying to write event (eventid = {0}) to event log.",
                    (int)AuditEventTypes.AuthorizationSuccess));
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
            if (customLog != null)
            {
                string AuthorizationFailed =
                    AuditEvents.AuthorizationFailed;
                string message = String.Format(AuthorizationFailed,
                    userName, serviceName, reason);
                customLog.WriteEntry(message);
            }
            else
            {
                throw new ArgumentException(string.Format("Error while trying to write event (eventid = {0}) to event log.",
                    (int)AuditEventTypes.AuthorizationFailure));
            }
        }
        #endregion

        #region TAJMER
        public static void PokretanjeTajmeraSucc(string userName)
        {
            if (customLog != null)
            {
                string UserPokretanjeSuccess = AuditEvents.PokretanjeTajmeraSucc;
                string message = String.Format(UserPokretanjeSuccess, userName);
                customLog.WriteEntry(message);
            }
            else
            {
                throw new ArgumentException(string.Format("Error while trying to write event (eventid = {0}) to event log.",
                    (int)AuditEventTypes.PokretanjeTajmeraSuc));
            }
        }

        public static void PokretanjeTajmeraFailed(string userName)
        {
            if (customLog != null)
            {
                string UserPokretanjeFailed = AuditEvents.PokretanjeTajmeraFailed;
                string message = String.Format(UserPokretanjeFailed, userName);
                customLog.WriteEntry(message);
            }
            else
            {
                throw new ArgumentException(string.Format("Error while trying to write event (eventid = {0}) to event log.",
                    (int)AuditEventTypes.PokretanjeTajmeraFail));
            }
        }

        public static void ZaustavljanjeTajmeraSucc(string userName)
        {
            if (customLog != null)
            {
                string UserZaustavljanjeSuccess = AuditEvents.ZaustavljanjeTajmeraSucc;
                string message = String.Format(UserZaustavljanjeSuccess, userName);
                customLog.WriteEntry(message);
            }
            else
            {
                throw new ArgumentException(string.Format("Error while trying to write event (eventid = {0}) to event log.",
                    (int)AuditEventTypes.ZaustavljanjeTajmeraSuc));
            }
        }

        public static void ZaustavljanjeTajmeraFailed(string userName)
        {
            if (customLog != null)
            {
                string UserZaustavljanjeFailed = AuditEvents.ZaustavljanjeTajmeraFailed;
                string message = String.Format(UserZaustavljanjeFailed, userName);
                customLog.WriteEntry(message);
            }
            else
            {
                throw new ArgumentException(string.Format("Error while trying to write event (eventid = {0}) to event log.",
                    (int)AuditEventTypes.ZaustavljanjeTajmeraFail));
            }
        }
        public static void PostavljanjeTajmeraSucc(string userName)
        {
            if (customLog != null)
            {
                string UserPostavljanjeSuccess = AuditEvents.PostavljanjeTajmeraSucc;
                string message = String.Format(UserPostavljanjeSuccess, userName);
                customLog.WriteEntry(message);
            }
            else
            {
                throw new ArgumentException(string.Format("Error while trying to write event (eventid = {0}) to event log.",
                    (int)AuditEventTypes.PostavljanjeTajmeraSuc));
            }
        }

        public static void PostavljanjeTajmeraFailed(string userName)
        {
            if (customLog != null)
            {
                string UserPostavljanjeFailed = AuditEvents.PostavljanjeTajmeraFailed;
                string message = String.Format(UserPostavljanjeFailed, userName);
                customLog.WriteEntry(message);
            }
            else
            {
                throw new ArgumentException(string.Format("Error while trying to write event (eventid = {0}) to event log.",
                    (int)AuditEventTypes.PostavljanjeTajmeraFail));
            }
        }

        public static void PonistavanjeTajmeraSucc(string userName)
        {
            if (customLog != null)
            {
                string UserPonistavanjeSuccess = AuditEvents.PonistavanjeTajmeraSucc;
                string message = String.Format(UserPonistavanjeSuccess, userName);
                customLog.WriteEntry(message);
            }
            else
            {
                throw new ArgumentException(string.Format("Error while trying to write event (eventid = {0}) to event log.",
                    (int)AuditEventTypes.PonistavanjeTajmeraSuc));
            }
        }

        public static void PonistavanjeTajmeraFailed(string userName)
        {
            if (customLog != null)
            {
                string UserPonistavanjeFailed = AuditEvents.PonistavanjeTajmeraFailed;
                string message = String.Format(UserPonistavanjeFailed, userName);
                customLog.WriteEntry(message);
            }
            else
            {
                throw new ArgumentException(string.Format("Error while trying to write event (eventid = {0}) to event log.",
                    (int)AuditEventTypes.PonistavanjeTajmeraFail));
            }
        }
        #endregion


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
