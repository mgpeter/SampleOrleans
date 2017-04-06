using System;

using Orleans;
using Orleans.Runtime.Configuration;
using SampleOrleans.GrainInterfaces;

namespace SampleOrleans.SiloHost
{
    /// <summary>
    /// Orleans test silo host
    /// </summary>
    public class Program
    {
        static void Main(string[] args)
        {
            // The Orleans silo environment is initialized in its own app domain in order to more
            // closely emulate the distributed situation, when the client and the server cannot
            // pass data via shared memory.
            AppDomain hostDomain = AppDomain.CreateDomain("OrleansHost", null, new AppDomainSetup
            {
                AppDomainInitializer = InitSilo,
                AppDomainInitializerArguments = args,
            });

            ClientConfiguration config = ClientConfiguration.LocalhostSilo();
            
            GrainClient.Initialize(config);

            ConsoleKeyInfo info;
            do
            {
                for (int i = 0; i < 500; i++)
                {
                    var name = $"BOT_{i}";
                    var randomizer = GrainClient.GrainFactory.GetGrain<IRandomizerGrain>(i % 25);
                    var task = randomizer.SayRandomWelcome(name);
                    task.Wait();
                    var result = task.Result;
                    Console.WriteLine($"\n\n{result}\n");
                }

                Console.WriteLine("Exit [ESC] ? Or press any other key to continue...");
                info = Console.ReadKey(true);
            } while (info.Key != ConsoleKey.Escape);

            Console.WriteLine("Orleans Silo is still running.\nPress Enter to terminate...");
            Console.ReadLine();

            hostDomain.DoCallBack(ShutdownSilo);
        }

        static void InitSilo(string[] args)
        {
            hostWrapper = new OrleansHostWrapper(args);

            if (!hostWrapper.Run())
            {
                Console.Error.WriteLine("Failed to initialize Orleans silo");
            }
        }

        static void ShutdownSilo()
        {
            if (hostWrapper != null)
            {
                hostWrapper.Dispose();
                GC.SuppressFinalize(hostWrapper);
            }
        }

        private static OrleansHostWrapper hostWrapper;
    }
}
