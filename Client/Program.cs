using Client.Connections;
using Common.ATMInterfaces;
using Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using HashAlgorithm = Manager.HashAlgorithm;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.ReadKey();
            ClientSCSProxy clientSCSProxy = new ClientSCSProxy();
        
            X509Certificate2 cert = CertManager.GetCertificateFromStorage(StoreName.My, StoreLocation.LocalMachine, Formatter.ParseName(WindowsIdentity.GetCurrent().Name));
            if (cert == null)
            {
                int option = 0;
                while (option != 2)
                {
                    Console.WriteLine("Do you want to make SmartCard?\n\t1. Yes\n\t2. No");
                    try
                    {
                        option = Int32.Parse(Console.ReadLine());
                        if (option == 1)
                        {
                            if (clientSCSProxy.CertificateIssue())
                            {
                                Console.WriteLine("Your SmartCard is successfully created. Please install it and press any key when you are done.");
                                Console.ReadKey();
                                break;
                            }
                        }
                        else if (option == 2)
                        {
                            Console.WriteLine("Goodbye. Press any key to exit.");
                            break;
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
            }
            else
            {
                ClientATMProxy clientATMProxy = new ClientATMProxy();
                X509Certificate2 certSign = CertManager.GetCertificateFromStorage(StoreName.My, StoreLocation.LocalMachine, Formatter.ParseName(WindowsIdentity.GetCurrent().Name + "Sign"));
                if (certSign != null)
                {
                    int option = 0;
                    while (option != 5)
                    {
                        Console.WriteLine("\nInsert PIN : ");

                        string pin = Console.ReadLine();
                        byte[] signPin = DigitalSignature.Create(pin, HashAlgorithm.SHA1, certSign);

                        string username = Formatter.ParseName(WindowsIdentity.GetCurrent().Name);
                        byte[] signUsername = DigitalSignature.Create(username, HashAlgorithm.SHA1, certSign);

                        try
                        {
                            if (clientATMProxy.ValidatePIN(pin, signPin, username, signUsername))
                            {
                                while (option != 5)
                                {
                                    Console.WriteLine("\nDo you want to:\n\t1. PayIn\n\t2. PayOut\n\t3. Print all users\n\t4. Change Pin\n\t5. Exit");
                                    try
                                    {
                                        option = Int32.Parse(Console.ReadLine());
                                        string amount;
                                        if (option == 1)
                                        {
                                            Console.WriteLine("Amount: ");
                                            amount = Console.ReadLine();
                                            try
                                            {
                                                byte[] signAmount = DigitalSignature.Create(amount, Manager.HashAlgorithm.SHA1, certSign);
                                                Console.WriteLine(clientATMProxy.PayIn(username, signUsername, amount, signAmount));
                                            }
                                            catch (Exception e)
                                            {
                                                Console.WriteLine(e.Message);
                                            }
                                        }
                                        else if (option == 2)
                                        {
                                            Console.WriteLine("Amount: ");
                                            amount = Console.ReadLine();

                                            byte[] signAmount = DigitalSignature.Create(amount, Manager.HashAlgorithm.SHA1, certSign);
                                            Console.WriteLine(clientATMProxy.PayOut(username, signUsername, amount, signAmount));
                                        }
                                        else if (option == 3)
                                        {
                                            Console.WriteLine(clientATMProxy.PrintAllClients());
                                        }
                                        else if (option == 4)
                                        {
                                            if (clientSCSProxy.ChangePin(username))
                                            {
                                                Console.WriteLine("Change PIN successfully executed.");
                                                break;
                                            }
                                            else
                                            {
                                                Console.WriteLine("Change PIN failed.");
                                            }
                                        }
                                    }
                                    catch (Exception e)
                                    {
                                        Console.WriteLine(e.Message);
                                    }
                                }
                            }
                            else
                            {
                                Console.WriteLine("Invalid PIN.");
                            }
                        }
                        catch
                        {
                            Console.WriteLine("ACCES DENIED.");
                            break;
                        }
                    }
                }
                else
                {
                    Console.WriteLine("User does not have sign certificate (or manage private keys).");
                }
            }
        
            Console.WriteLine("Goodbye. Press any key to exit.");
            Console.ReadKey();
        }
    }
}
