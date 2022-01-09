using Common;
using Manager;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Permissions;
using System.Security.Principal;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Service
{
    public class WCFService : IWCFService
    {
        private readonly Stopwatch Stopwatch = new Stopwatch();
        private double Timer = 0;

        public static Dictionary<string, User> UserAccountsDB = new Dictionary<string, User>();


        #region Metode
     
        public bool PokreniTimer()
        {
            CustomPrincipal principal = Thread.CurrentPrincipal as CustomPrincipal;
            string userName = Formatter.ParseName(principal.Identity.Name);

            if (Thread.CurrentPrincipal.IsInRole("StartStop"))
            {
                try
                {
                    Stopwatch.Start();
                    Console.WriteLine("Tajmer pokrenut!");
                    Audit.AuthorizationSuccess(userName, OperationContext.Current.IncomingMessageHeaders.Action);
                    Audit.PokretanjeTajmeraSucc(userName);
                    return true;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return false;
                }
            }
            else
            {
                try
                {
                    Audit.AuthorizationFailed(userName, OperationContext.Current.IncomingMessageHeaders.Action, $"{nameof(PokreniTimer)} method needs StartStop permission.");
                    Audit.PokretanjeTajmeraFailed(userName);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }

                throw new FaultException($"User {userName} tried to call {nameof(PokreniTimer)} method. {nameof(PokreniTimer)} method needs StartStop permission.");
            }
        }

        public bool ZaustaviTimer()
        {
            CustomPrincipal principal = Thread.CurrentPrincipal as CustomPrincipal;
            string userName = Formatter.ParseName(principal.Identity.Name);

            if (Thread.CurrentPrincipal.IsInRole("StartStop"))
            {
                try
                {
                    Stopwatch.Stop();
                    Console.WriteLine("Tajmer zaustavljen!");
                    Audit.ZaustavljanjeTajmeraSucc(userName);
                    return true;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return false;
                }
            }
            else
            {
                try
                {
                    Audit.AuthorizationFailed(userName, OperationContext.Current.IncomingMessageHeaders.Action, $"{nameof(ZaustaviTimer)} method needs StartStop permission.");
                    Audit.ZaustavljanjeTajmeraFailed(userName);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }

                throw new FaultException($"User {userName} tried to call {nameof(ZaustaviTimer)} method. {nameof(ZaustaviTimer)} method needs StartStop permission.");
            }
        }


        public bool PonistiTimer()
        {
            CustomPrincipal principal = Thread.CurrentPrincipal as CustomPrincipal;
            string userName = Formatter.ParseName(principal.Identity.Name);

            if (Thread.CurrentPrincipal.IsInRole("Change"))
            {
                try
                {
                    Timer = 0;
                    Stopwatch.Restart();
                    Console.WriteLine("Tajmer ponisten!");
                    Audit.PonistavanjeTajmeraSucc(userName);
                    return true;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return false;
                }
            }
            else
            {
                try
                {
                    Audit.AuthorizationFailed(userName, OperationContext.Current.IncomingMessageHeaders.Action, $"{nameof(PonistiTimer)} method need Change permission.");
                    Audit.PonistavanjeTajmeraFailed(userName);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }

                throw new FaultException($"User {userName} tried to call {nameof(PonistiTimer)} method. {nameof(PonistiTimer)} method needs Change permission.");
            }
        }

        public bool PostaviTimer(byte[] CipheredTimerMax)
        {
            CustomPrincipal principal = Thread.CurrentPrincipal as CustomPrincipal;
            string userName = Formatter.ParseName(principal.Identity.Name);

            if (Thread.CurrentPrincipal.IsInRole("Change"))
            {
                try
                {
                    Timer = DecryptNumber(CipheredTimerMax, SecretKey.LoadKey("SecretKey.txt"), CipherMode.CBC);
                    Console.WriteLine($"Tajmer je postavljen na {Timer} sekundi.");
                    Audit.PostavljanjeTajmeraSucc(userName);
                    return true;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return false;
                }
            }
            else
            {
                try
                {
                    Audit.AuthorizationFailed(userName, OperationContext.Current.IncomingMessageHeaders.Action, $"{nameof(PostaviTimer)} method need Change permission.");
                    Audit.PostavljanjeTajmeraFailed(userName);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }

                throw new FaultException($"User {userName} tried to call {nameof(PostaviTimer)} method. {nameof(PostaviTimer)} method needs Change permission.");
            }
        }

   
        public string OcitajTimer()
        {
            if (!Thread.CurrentPrincipal.IsInRole("See"))
                return string.Empty;

            double timeLeft = Timer - Stopwatch.ElapsedMilliseconds / 1000;
            if (timeLeft <= 0)
            {
                Stopwatch.Stop();
                Stopwatch.Restart();
                Timer = 0;
                return "Nije preostalo vremena!";
            }
            else
                return $"Prestalo je: {timeLeft} sekundi.";
        }
        #endregion

        #region ManagePermissions
        //[PrincipalPermission(SecurityAction.Demand, Role = "Administrate")]
        public bool ManagePermission(bool isAdd, string rolename, params string[] permissions)
        {
            CustomPrincipal principal = Thread.CurrentPrincipal as CustomPrincipal;
            string userName = Formatter.ParseName(principal.Identity.Name);

            if (Thread.CurrentPrincipal.IsInRole("Administrate"))
            {
                try
                {
                    if (isAdd)
                        RolesConfig.AddPermissions(rolename, permissions);
                    else
                        RolesConfig.RemovePermissions(rolename, permissions);
                    return true;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return false;
                }
            }
            else
            {
                try
                {
                    Audit.AuthorizationFailed(userName,
                        OperationContext.Current.IncomingMessageHeaders.Action, $"{nameof(ManagePermission)} method need Administrate permission.");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }

                throw new FaultException($"User {userName} tried to call {nameof(ManagePermission)} method. {nameof(ManagePermission)} method needs Administrate permission.");
            }
        }

        //[PrincipalPermission(SecurityAction.Demand, Role = "Administrate")]
        public bool ManageRoles(bool isAdd, string rolename)
        {
            CustomPrincipal principal = Thread.CurrentPrincipal as CustomPrincipal;
            string userName = Formatter.ParseName(principal.Identity.Name);

            if (Thread.CurrentPrincipal.IsInRole("Administrate"))
            {
                try
                {
                    if (isAdd)
                        RolesConfig.AddRole(rolename);
                    else
                        RolesConfig.RemoveRole(rolename);
                    return true;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return false;
                }
            }
            else
            {
                try
                {
                    Audit.AuthorizationFailed(userName,
                        OperationContext.Current.IncomingMessageHeaders.Action, $"{nameof(ManagePermission)} method need Administrate permission.");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }

                throw new FaultException($"User {userName} tried to call {nameof(ManagePermission)} method. {nameof(ManagePermission)} method needs Administrate permission.");
            }
        }
        #endregion

       
        private static double DecryptNumber(byte[] encryptedNumber, string secretKey, CipherMode mode)
        {
            byte[] number = null;

            DESCryptoServiceProvider desCryptoProvider = new DESCryptoServiceProvider
            {
                Key = ASCIIEncoding.ASCII.GetBytes(secretKey),
                Mode = mode,
                Padding = PaddingMode.PKCS7
            };

            if (mode.Equals(CipherMode.CBC))
            {
                desCryptoProvider.IV = encryptedNumber.Take(desCryptoProvider.BlockSize / 8).ToArray();                // take the iv off the beginning of the ciphertext message			

                ICryptoTransform desDecryptTransform = desCryptoProvider.CreateDecryptor();

                using (MemoryStream memoryStream = new MemoryStream(encryptedNumber.Skip(desCryptoProvider.BlockSize / 8).ToArray()))
                {
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, desDecryptTransform, CryptoStreamMode.Read))
                    {
                        number = new byte[encryptedNumber.Length - desCryptoProvider.BlockSize / 8];     //decrypted image body - the same lenght as encrypted part
                        cryptoStream.Read(number, 0, number.Length);
                    }
                }
            }
            return double.Parse(ASCIIEncoding.ASCII.GetString(number));
        }
    }
}
