using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace Manager
{
	public enum AuditEventTypes
	{
		AuthenticationSuccess = 0,
		AuthorizationSuccess = 1,
		AuthorizationFailure = 2,
		PokretanjeTajmeraSuc = 3,
		ZaustavljanjeTajmeraSuc = 4,
		PostavljanjeTajmeraSuc = 5,
		PonistavanjeTajmeraSuc = 6,
		PokretanjeTajmeraFail = 7,
		ZaustavljanjeTajmeraFail = 8,
		PostavljanjeTajmeraFail = 9,
		PonistavanjeTajmeraFail = 10

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
						resourceManager = new ResourceManager
							(typeof(AuditEventFile).ToString(),
							Assembly.GetExecutingAssembly());
					}
					return resourceManager;
				}
			}
		}
        #region AAA
        public static string AuthenticationSuccess
		{
			get
			{
				// TO DO
				return ResourceMgr.GetString(AuditEventTypes.AuthenticationSuccess.ToString());
			}
		}

		public static string AuthorizationSuccess
		{
			get
			{
				//TO DO
				return ResourceMgr.GetString(AuditEventTypes.AuthorizationSuccess.ToString());
			}
		}

		public static string AuthorizationFailed
		{
			get
			{
				//TO DO
				return ResourceMgr.GetString(AuditEventTypes.AuthorizationFailure.ToString());
			}
		}
        #endregion

        #region TAJMER
        public static string PokretanjeTajmeraSucc
		{
			get
			{
				return ResourceMgr.GetString(AuditEventTypes.PokretanjeTajmeraSuc.ToString());
			}
		}

		public static string PokretanjeTajmeraFailed
		{
			get
			{
				return ResourceMgr.GetString(AuditEventTypes.PokretanjeTajmeraFail.ToString());
			}
		}
		public static string ZaustavljanjeTajmeraSucc
		{
			get
			{
				return ResourceMgr.GetString(AuditEventTypes.ZaustavljanjeTajmeraSuc.ToString());
			}
		}

		public static string ZaustavljanjeTajmeraFailed
		{ 
			get
			{
				return ResourceMgr.GetString(AuditEventTypes.ZaustavljanjeTajmeraFail.ToString());
			}
		}

		public static string PostavljanjeTajmeraSucc
		{
			get
			{
				return ResourceMgr.GetString(AuditEventTypes.PostavljanjeTajmeraSuc.ToString());
			}
		}
		public static string PostavljanjeTajmeraFailed
		{
			get
			{
				return ResourceMgr.GetString(AuditEventTypes.PostavljanjeTajmeraFail.ToString());
			}
		}
		public static string PonistavanjeTajmeraSucc
		{
			get
			{
				return ResourceMgr.GetString(AuditEventTypes.PonistavanjeTajmeraSuc.ToString());
			}
		}

		public static string PonistavanjeTajmeraFailed
		{
			get
			{
				return ResourceMgr.GetString(AuditEventTypes.PonistavanjeTajmeraFail.ToString());
			}
		}
		#endregion
	}
}
