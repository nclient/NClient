using System;
using System.Collections.Concurrent;
using System.Linq;

namespace NClient.Testing.Common.Helpers
{
    public static class PortsPool
    {
        private static readonly ConcurrentBag<int> Ports;
        
        static PortsPool()
        {
            Ports = new ConcurrentBag<int>(Enumerable.Range(start: 5100, count: 900));
        }
        
        public static int Get()
        {
            if (Ports.TryTake(out var port))
                return port;
            
            throw new InvalidOperationException("The port cannot be returned. The ports in the pool have run out.");
        }

        public static void Put(int port)
        {
            Ports.Add(port);
        }
    }
}
