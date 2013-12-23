using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.IO;
using NDesk.Options;

namespace ActiveConnections
{
    class Program
    {
        private static string GetFileName()
        {
            string codeBase = Assembly.GetExecutingAssembly().CodeBase;
            return Path.GetFileName(codeBase);
        }

        private static void Main(string[] args)
        {
            bool show_help = false;
            int num = 100;
            string fileName = GetFileName();

            var p = new OptionSet() {
                { "n|numeric=", "This option sets the minimum count of active network connections.", v => num = Convert.ToInt32(v) },
                { "h|?|help",  "Show help.", v => show_help = v != null },
            };

            List<string> extra;
            try
            {
                extra = p.Parse(args);
            }
            catch (OptionException e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(string.Format("Try `{0} --help' for more information.", fileName));
                return;
            }

            if (show_help)
            {
                ShowHelp(p);
                return;
            }


            Netstat netState = new Netstat();
            Netstat.NetstateData netStateData = netState.GetConnections(num);

            Console.WriteLine("Total Connection Count: " + netStateData.TotalCount);
            if (netStateData.Addresses.Count > 0)
            {
                Console.WriteLine("\nAddress\tCount");
                foreach (KeyValuePair<string, int> item in netStateData.Addresses)
                {
                    Console.WriteLine(item.Key + "\t" + item.Value);
                }
            }

            if (netStateData.Ports.Count > 0)
            {
                Console.WriteLine("\nPort\tCount");
                foreach (KeyValuePair<string, int> item in netStateData.Ports)
                {
                    Console.WriteLine(item.Key + "\t" + item.Value);
                }
            }
        }

        static void ShowHelp(OptionSet p)
        {
            Console.WriteLine(string.Format("Usage: {0} [-n|numeric] [-h|help]", GetFileName()));
            Console.WriteLine("Print active/established network connections.");
            Console.WriteLine();
            Console.WriteLine("Options:");
            p.WriteOptionDescriptions(Console.Out);
        }
    }
}
