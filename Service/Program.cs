using Common;
using Manager;
using System;
using System.Collections.Generic;
using System.IdentityModel.Policy;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Principal;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class Program
    {
        static void Main(string[] args)
        {
            NetTcpBinding binding = new NetTcpBinding();
            string address = "net.tcp://localhost:9999/WCFService";

            binding.Security.Mode = SecurityMode.Transport;
            binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Windows;
            binding.Security.Transport.ProtectionLevel = System.Net.Security.ProtectionLevel.EncryptAndSign;

            ServiceHost host = new ServiceHost(typeof(WCFService));
            host.AddServiceEndpoint(typeof(IWCFService), binding, address);

            // podesavamo da se koristi MyAuthorizationManager umesto ugradjenog
            host.Authorization.ServiceAuthorizationManager = new CustomAuthorizationManager();

            // podesavamo custom polisu, odnosno nas objekat principala
            host.Authorization.PrincipalPermissionMode = PrincipalPermissionMode.Custom;
            List<IAuthorizationPolicy> policies = new List<IAuthorizationPolicy>();
            policies.Add(new CustomAuthorizationPolicy());
            host.Authorization.ExternalAuthorizationPolicies = policies.AsReadOnly();

            //podesavanje AuditBehaviour-a
            ServiceSecurityAuditBehavior newAudit = new ServiceSecurityAuditBehavior();
            newAudit.AuditLogLocation = AuditLogLocation.Application;
            newAudit.ServiceAuthorizationAuditLevel = AuditLevel.SuccessOrFailure;

            host.Description.Behaviors.Remove<ServiceSecurityAuditBehavior>();
            host.Description.Behaviors.Add(newAudit);

            host.Open();
            Console.WriteLine("Korisnik koji je pokrenuo server :" + WindowsIdentity.GetCurrent().Name);

            Console.WriteLine("Servis je pokrenut.");

            /*
            string keyFile = "SecretKey.txt";     //secret key storage
            string folderNameDES = "DES/";
            //string cipherFileCBC = "CipheredCBC.bmp";			//result of ECB encryption
            //string plaintextFileCBC = "PlaintextCBC.bmp";		//result of ECB decryption
            string eSecretKeyDes = SecretKey.GenerateKey(AlgorithmType.DES);
            SecretKey.StoreKey(eSecretKeyDes, folderNameDES + keyFile);
            Test_DES_Encrypt(broj, folderNameDES + cipherFileCBC, SecretKey.LoadKey(folderNameDES + keyFile), CipherMode.CBC);
            Console.WriteLine("Encryption is done.");
            Test_DES_Decrypt(folderNameDES + cipherFileCBC, folderNameDES + plaintextFileCBC, SecretKey.LoadKey(folderNameDES + keyFile), CipherMode.CBC);
            Console.WriteLine("Decryption is done.");
    */        
            Console.ReadLine();

            host.Close();
        }
            
            #region DES Alogirthm

        static void Test_DES_Encrypt(string inputNumber, string outputNumber, string secretKey, CipherMode mode)
        {
            try
            {
                DES_Symm_Algorithm.EncryptFile(inputNumber, outputNumber, secretKey, mode);
                Console.WriteLine("The number is successfully encrypted.");
            }
            catch (Exception e)
            {
                Console.WriteLine("Encryption failed. Reason: {0}", e.Message);
            }
        }

        static void Test_DES_Decrypt(string inputNumber, string outputNumber, string secretKey, CipherMode mode)
        {
            try
            {
                DES_Symm_Algorithm.DecryptFile(inputNumber, outputNumber, secretKey, mode);
                Console.WriteLine("The number on location is successfully decrypted.");
            }
            catch (Exception e)
            {
                Console.WriteLine("Decryption failed. Reason: {0}", e.Message);
            }
        }

        #endregion
    }
}
