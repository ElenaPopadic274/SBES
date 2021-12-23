using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public class WCFClient : ChannelFactory<IWCFService>, IWCFService, IDisposable
	{
		IWCFService factory;

		public WCFClient(NetTcpBinding binding, EndpointAddress address)
			: base(binding, address)
		{
			factory = this.CreateChannel();
			//Credentials.Windows.AllowNtlm = false;
		}


		public void AddUser(string username, string password)
		{

			try
			{
				factory.AddUser(username, password);
			}
			catch (Exception e)
			{
				Console.WriteLine("Error: {0}", e.Message);
			}
		}


		#region ManagePermiossions
		public void ManagePermission(bool isAdd, string rolename, params string[] permissions)
		{
			try
			{
				factory.ManagePermission(isAdd, rolename, permissions);
				Console.WriteLine("Manage allowed");
			}
			catch (Exception e)
			{
				Console.WriteLine("Error while trying to Manage : {0}", e.Message);
			}
		}

		public void ManageRoles(bool isAdd, string rolename)
		{
			try
			{
				factory.ManageRoles(isAdd, rolename);
				Console.WriteLine("Manage allowed");
			}
			catch (Exception e)
			{
				Console.WriteLine("Error while trying to Manage : {0}", e.Message);
			}
		}
		#endregion
		public void Dispose()
		{
			if (factory != null)
			{
				factory = null;
			}

			this.Close();
		}

		public bool PokreniTimer(int key)
		{
			throw new NotImplementedException();
		}

		public bool ZaustaviTimer(int key)
		{
			throw new NotImplementedException();
		}

		public bool PonistiTimer(int key, Stopwatch st)
		{
			throw new NotImplementedException();
		}

		public bool PostaviTimer(int key, Stopwatch st)
		{
			throw new NotImplementedException();
		}

		public bool OcitajTimer(int key)
		{
			throw new NotImplementedException();
		}
	}
}
