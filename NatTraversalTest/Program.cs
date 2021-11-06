using System;
using System.Linq;
using System.Threading.Tasks;
using Open.Nat;

namespace NatTraversalTest
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var v = new Open.Nat.NatDiscoverer();
            
            var d = await v.DiscoverDeviceAsync();
            Console.ReadLine();
            Console.WriteLine(d.GetExternalIPAsync());
            Console.ReadLine();
        }
    }
}