using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Extreme._Extensions
{
    internal static class TcpClientExtensions
    {

        public static void Close(this TcpClient client)
        {
            ((IDisposable)client).Dispose();
        }

    }
}
