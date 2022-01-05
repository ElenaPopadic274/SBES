using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Client
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

            Console.WriteLine("Korisnik koji je pokrenuo klijenta je : " + WindowsIdentity.GetCurrent().Name);


            using (WCFClient proxy = new WCFClient(binding, new EndpointAddress(new Uri(address), EndpointIdentity.CreateUpnIdentity("wcfServer"))))
            {
                while (true)
                {
                    Console.WriteLine("Izaberite opciju: ");
                    Console.WriteLine("1 - Pokreni timer.");
                    Console.WriteLine("2 - Zaustavi timer.");
                    Console.WriteLine("3 - Postavi timer.");
                    Console.WriteLine("4 - Ponisti timer.");
                    Console.WriteLine("5 - Ocitaj timer.");
                    Console.WriteLine("--------------------");
                    Console.WriteLine("6 - Rad sa dozvolama!");
                    Console.WriteLine("7 - Rad sa ulogama!");
                    Console.WriteLine("Ako se unese nesto drugo, klijent se gasi!");
                    Console.WriteLine("--------------------");

                    switch (Console.ReadLine())
                    {
                        case "1":
                            {
                                if (proxy.PokreniTimer())
                                    Console.WriteLine("Tajmer uspesno pokrenut!");
                                else
                                    Console.WriteLine("Tajmer nije uspesno pokrenut!");
                                break;
                            }
                        case "2":
                            {
                                if (proxy.ZaustaviTimer())
                                {
                                    Console.WriteLine("Tajmer uspesno zaustavljen!");
                                }
                                else
                                {
                                    Console.WriteLine("Tajmer nije uspesno zaustavljen!");
                                }
                                break;
                            }
                        case "3":
                            {
                                Console.WriteLine("Unesite vreme: ");
                                if (proxy.PostaviTimer(Encoding.ASCII.GetBytes(Console.ReadLine())))
                                {
                                    Console.WriteLine("Tajmer uspesno postavljen na unetu vrednost!");
                                }
                                else
                                {
                                    Console.WriteLine("Tajmer nije uspesno postavljen!");
                                }
                                break;
                            }
                        case "4":
                            {
                                if (proxy.PonistiTimer())
                                    Console.WriteLine("Tajmer uspesno ponisten!");
                                else
                                    Console.WriteLine("Tajmer nije uspesno ponisten!");
                                break;
                            }
                        case "5":
                            Console.WriteLine(proxy.OcitajTimer());
                            break;
                        case "6":
                            {
                                Console.WriteLine("Naziv grupe: ");
                                string nazivGrupe = Console.ReadLine();
                                Console.WriteLine("Dozvole(razdvojene zarezom): ");
                                string[] dozvole = Console.ReadLine().Split(',');

                                Console.WriteLine("Dodavanje(1) ili Oduzimanje(0)?");

                                if (proxy.ManagePermission(Console.ReadLine() == "1", nazivGrupe, dozvole))
                                    Console.WriteLine($"Uspesno je izvrsen zahtev.");
                                else
                                    Console.WriteLine($"Zahtev nije izvrsen uspesno!");
                                break;
                            }
                        case "7":
                            {
                                Console.WriteLine("Naziv grupe: ");
                                string nazivGrupe = Console.ReadLine();
                                Console.WriteLine("Dodavanje(1) ili Brisanje(0)?");

                                if (proxy.ManageRoles(Console.ReadLine() == "1", nazivGrupe))
                                    Console.WriteLine($"Uspesno je izvrsen zahtev.");
                                else
                                    Console.WriteLine($"Zahtev nije izvrsen uspesno!");
                                break;
                            }
                        default:
                            {
                                Console.WriteLine("Klijent ugasen!");
                                proxy.Close();
                                return;
                            }
                    }
                }
            }
        }
    }
}
