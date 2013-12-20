using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.IO;
using System.Collections;
using System.Text.RegularExpressions;

namespace ActiveConnections
{
    class Netstate
    {
        public struct NetstateData
        {
            public int TotalCount;
            public List<KeyValuePair<string, int>> Addresses;
            public List<KeyValuePair<string, int>> Ports;
        }

        public NetstateData GetConnections(int num)
        {
            NetstateData netStateData = new NetstateData();

            ProcessStartInfo startInfo = new ProcessStartInfo("netstat", "-na");
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardInput = true;
            startInfo.RedirectStandardOutput = true;
            startInfo.CreateNoWindow = true;

            Process process = Process.Start(startInfo);
            StreamReader standardOutput = process.StandardOutput;
            for (int i = 0; i < 4; i++)
            {
                process.StandardOutput.ReadLine();
            }

            int totalCount = 0;
            Dictionary<string, int> dictAddress = new Dictionary<string, int>();
            Dictionary<string, int> dictPort = new Dictionary<string, int>();

            while (true)
            {
                ++totalCount;
                string str = process.StandardOutput.ReadLine();
                if (str != null)
                {

                    string line = str.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)[2];
                    Match match = Regex.Match(line, "^(.*):(\\d{1,})");

                    if (match.Success)
                    {
                        string[] data = new string[] { match.Groups[1].ToString(), match.Groups[2].ToString() };

                        string address = data[0];
                        string port = data[1];

                        try
                        {
                            ++dictAddress[address];
                        }
                        catch (KeyNotFoundException)
                        {
                            dictAddress[address] = 1;
                        }

                        try
                        {
                            ++dictPort[port];
                            continue;
                        }
                        catch (KeyNotFoundException)
                        {
                            dictPort[port] = 1;
                            continue;
                        }
                    }
                }
                break;
            }

            netStateData.TotalCount = totalCount;
            netStateData.Addresses = new List<KeyValuePair<string, int>>();
            foreach (KeyValuePair<string, int> item in dictAddress)
            {
                if (item.Value > num)
                {
                    netStateData.Addresses.Add(item);
                }
            }

            netStateData.Ports = new List<KeyValuePair<string, int>>();
            foreach (KeyValuePair<string, int> item in dictPort)
            {
                if (item.Value > num)
                {
                    netStateData.Ports.Add(item);
                }
            }

            return netStateData;
        }
    }
}
