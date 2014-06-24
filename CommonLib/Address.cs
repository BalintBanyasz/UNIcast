using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace CommonLib
{
    public class Address
    {
        private const string PtrnAddress = @"^(?<ip>\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}):(?<port>\d{1,5})$";
        public string IP;
        public int Port;

        public Address(string ip, int port)
        {
            this.IP = ip;
            this.Port = port;
        }

        public override string ToString()
        {
            return String.Format("{0}:{1}", IP, Port);
        }

        public static bool TryParse(string str, out Address address)
        {
            Match match = Regex.Match(str, PtrnAddress);
            if (match.Success)
            {
                address = new Address(match.Groups["ip"].Value, Int32.Parse(match.Groups["port"].Value));
                return true;
            }
            else
            {
                address = null;
                return false;
            }
        }
    }
}
