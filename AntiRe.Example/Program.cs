using System;
using AntiRE.Runtime;
using System.Diagnostics;
using System.Reflection;
using System.Net;

namespace AntiRe.Example
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "AntiRE.Example";
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine(@"

                 _   _ _____  ______   _____             _   _                
     /\         | | (_)  __ \|  ____| |  __ \           | | (_)               
    /  \   _ __ | |_ _| |__) | |__    | |__) |   _ _ __ | |_ _ _ __ ___   ___ 
   / /\ \ | '_ \| __| |  _  /|  __|   |  _  / | | | '_ \| __| | '_ ` _ \ / _ \
  / ____ \| | | | |_| | | \ \| |____ _| | \ \ |_| | | | | |_| | | | | | |  __/
 /_/    \_\_| |_|\__|_|_|  \_\______(_)_|  \_\__,_|_| |_|\__|_|_| |_| |_|\___|
                                                                              ");
            var CurrentProcess = Process.GetCurrentProcess();
            bool SelfDelete = false;
            bool ShowAlert = true;
            //Alert settings
            Alert.NotepadStyle = true;
            Alert.AutoClose = false;
            Alert.AutoCloseTime = 2;
            Alert.NotepadPath = "readme.txt";
            //Prevent assembly being dumped from memory
            AntiDump.Parse(typeof(Program /* or this.GetType() */));
            //Prevent application start under sandbox tools
            AntiSandBox.SelfDelete = SelfDelete;
            AntiSandBox.ShowAlert = ShowAlert;
            AntiSandBox.Parse(CurrentProcess);
            //Prevent application start under virtual machine
            AntiVirtualMachine.SelfDelete = SelfDelete;
            AntiVirtualMachine.ShowAlert = ShowAlert;
            AntiVirtualMachine.Parse(CurrentProcess);
            //Prevent from network being monitored
            AntiSniff.SelfDelete = SelfDelete;
            AntiSniff.ShowAlert = ShowAlert;
            AntiSniff.Parse(CurrentProcess);
            //Prevents reverse engineering tools from running in the system
            AntiReverserTools.SelfDelete = SelfDelete;
            AntiReverserTools.ShowAlert = ShowAlert;
            AntiReverserTools.IgnoreCase = true;
            AntiReverserTools.KeepAlive = true;
            AntiReverserTools.WhiteList.Add("notepad");
            AntiReverserTools.BlackList.Add("dnspy");
            AntiReverserTools.Start(CurrentProcess);
            //Anti debugger
            AntiDebugger.SelfDelete = SelfDelete;
            AntiDebugger.ShowAlert = ShowAlert;
            AntiDebugger.KeepAlive = true;
            AntiDebugger.Start(CurrentProcess);
            //Detect if dnspy installed on system
            AntiDnspy.SelfDelete = SelfDelete;
            AntiDnspy.ShowAlert = ShowAlert;
            AntiDnspy.Parse(CurrentProcess);
            //Send anti sniff request to server
            try
            {
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create("https://google.com");
                req.ContinueTimeout = 10000;
                req.ReadWriteTimeout = 10000;
                req.Timeout = 10000;
                req.KeepAlive = true;
                req.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/28.0.1500.63 Safari/537.36";
                req.Accept = "*/*";
                req.Method = "GET";
                req.Headers.Add("Accept-Language", "en-US,en;q=0.9,fa;q=0.8");
                req.Headers.Add("Accept-Encoding", "gzip, deflate");
                req.AutomaticDecompression = DecompressionMethods.GZip;
                req.ServerCertificateValidationCallback = AntiSniff.ValidationCallback;
                req.ServicePoint.Expect100Continue = false;
                using (HttpWebResponse response = req.GetResponse() as HttpWebResponse)
                {
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        Alert.Show("NETWORK CONNECTION ERROR, CHECK YOUR INTERNET CONNECTION OR CLOSE SNIFFER SOFTWARES");
                        Environment.Exit(0);
                        return;
                    }
                }
            }
            catch
            {
                Alert.Show("NETWORK CONNECTION ERROR, CHECK YOUR INTERNET CONNECTION OR CLOSE SNIFFER SOFTWARES");
                Environment.Exit(0);
                return;
            }
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("\r\n [#] Application started successfully\r\n [#] Press any key to exit...");
            Console.ReadKey();
        }
    }
}
