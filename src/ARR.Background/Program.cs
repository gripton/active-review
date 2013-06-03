using System;
using ARR.Background.Events;
using Autofac;
using NLog;
using Topshelf;

namespace ARR.Background
{
    class Program
    {
        private static readonly Logger log = LogManager.GetLogger(typeof(Program).Name);
        
        static void Main(string[] args)
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule<BackgroundModule>();
            var container = builder.Build();

            HostFactory.Run(x =>
            {
                x.Service<IEventPublisherScheduler>(s =>
                {
                    s.ConstructUsing(name => container.Resolve<IEventPublisherScheduler>());
                    s.WhenStarted(tc => tc.Start());
                    s.WhenStopped(tc => tc.ShutDown());
                });
                x.RunAsLocalSystem();

                x.SetDescription("EventPublisherScheduler proudly hosted by TopShelf");
                x.SetDisplayName("Event Publisher Scheduler");
                x.SetServiceName("EventPublisherScheduler");
            });
        }
    }
}
