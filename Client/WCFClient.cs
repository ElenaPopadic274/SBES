using Common;
//using Manager;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public class WCFClient : ChannelFactory<IWCFService>, IWCFService, IDisposable
	{
		private IWCFService factory;

		public WCFClient(NetTcpBinding binding, EndpointAddress address)
			: base(binding, address)
		{
			factory = this.CreateChannel();
			//Credentials.Windows.AllowNtlm = false;
		}



		#region ManagePermiossions
		public bool ManagePermission(bool isAdd, string rolename, params string[] permissions)
		{
			try
			{
				return factory.ManagePermission(isAdd, rolename, permissions);
			}
			catch (Exception e)
			{
				Console.WriteLine("Error while trying to Manage : {0}", e.Message);
				return false;
			}
		}

		public bool ManageRoles(bool isAdd, string rolename)
		{
			try
			{
				return factory.ManageRoles(isAdd, rolename);
			}
			catch (Exception e)
			{
				Console.WriteLine("Error while trying to Manage : {0}", e.Message);
				return false;
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

		public bool PokreniTimer()
		{
			try
			{
				return factory.PokreniTimer();
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				return false;
			}
		}

		public bool ZaustaviTimer()
		{
			try
			{
				return factory.ZaustaviTimer();
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				return false;
			}
		}

		public bool PonistiTimer()
		{
			try
			{
				return factory.PonistiTimer();
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				return false;
			}
		}

		public bool PostaviTimer(byte[] CipheredTimerMax)
		{
			try
			{
				string eSecretKeyDes = SecretKey.GenerateKey(AlgorithmType.DES);
				SecretKey.StoreKey(eSecretKeyDes, "SecretKey.txt");
				return factory.PostaviTimer(EncryptNumber(CipheredTimerMax, eSecretKeyDes, CipherMode.CBC));
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				return false;
			}
		}

		public string OcitajTimer()
		{
			try
			{
				return factory.OcitajTimer();
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				return string.Empty;
			}
		}

		private byte[] EncryptNumber(byte[] numberToCipher, string secretKey, CipherMode mode)
		{
			byte[] encryptedNumber = null;

			DESCryptoServiceProvider desCryptoProvider = new DESCryptoServiceProvider
			{
				Key = ASCIIEncoding.ASCII.GetBytes(secretKey),
				Mode = mode,
				Padding = PaddingMode.PKCS7 //.none, pkcs7 standard, ne radi ako koristimo none izbacuje gresku
			};

			if (mode.Equals(CipherMode.CBC))
			{
				desCryptoProvider.GenerateIV();
				ICryptoTransform desEncryptTransform = desCryptoProvider.CreateEncryptor();

				using (MemoryStream memoryStream = new MemoryStream())
				{
					using (CryptoStream cryptoStream = new CryptoStream(memoryStream, desEncryptTransform, CryptoStreamMode.Write))
					{
						cryptoStream.Write(numberToCipher, 0, numberToCipher.Length);
					}
					encryptedNumber = desCryptoProvider.IV.Concat(memoryStream.ToArray()).ToArray(); //ako prebacimo u donji blok ne moze da cita memoriju
				}
			}
			return encryptedNumber;
		}
	}
}
