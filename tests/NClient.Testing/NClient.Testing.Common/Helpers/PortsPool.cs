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
            #if NET462
            Ports = new ConcurrentBag<int>(Enumerable.Range(start: 5000, count: 9));
            #elif NET472
            Ports = new ConcurrentBag<int>(Enumerable.Range(start: 5010, count: 9));
            #elif NET48
            Ports = new ConcurrentBag<int>(Enumerable.Range(start: 5020, count: 9));
            #elif NETCOREAPP3_1
            Ports = new ConcurrentBag<int>(Enumerable.Range(start: 5030, count: 9));
            #elif NET5_0
            Ports = new ConcurrentBag<int>(Enumerable.Range(start: 5040, count: 9));
            #else
            throw new NotSupportedException();
            #endif
        }
        
        public static int Get()
        {
            if (Ports.TryTake(out var port))
            {
                Console.WriteLine($"{nameof(PortsPool)}: Port {port} is selected.");
                return port;
            }
            
            throw new InvalidOperationException("The port cannot be returned. The ports in the pool have run out.");
        }

        public static void Put(int port)
        {
            Ports.Add(port);
        }
    }
}
