using CQELight;
using CQELight.Dispatcher;
using HelloWorld.Events;
using System;
using System.Threading.Tasks;

namespace HelloWorld
{
    static class Program
    {
        static async Task Main(string[] args)
        {
            new Bootstrapper(new BootstrapperOptions { AutoLoad = true }).Bootstrapp();

            await CoreDispatcher.PublishEventAsync(new GreetingsEvent()).ConfigureAwait(false);

            Console.Read();
        }
    }
}
