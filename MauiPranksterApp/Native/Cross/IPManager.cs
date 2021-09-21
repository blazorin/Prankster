using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MauiPranksterApp.Native.Cross
{
	public static class IPManager
	{
        public static string GetLocalAddress()
        {
            var IpAddress = Dns.GetHostAddresses(Dns.GetHostName()).FirstOrDefault();

            if (IpAddress != null)
                return IpAddress.ToString();

            return "255.255.255.255";
        }
    }
}
