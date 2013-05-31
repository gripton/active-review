using System;
using NLog;
using Topshelf;

namespace ARR.Background
{
    class Program
    {
        private static readonly Logger log = LogManager.GetLogger(typeof(Program).Name);
        
        static void Main(string[] args)
        {
            HostFactory.Run(x =>
            {
                x.Service<ILogService>(s =>
                {
                    s.ConstructUsing(name => new LogService());
                    s.WhenStarted(tc => tc.Start());
                    s.WhenStopped(tc => tc.Stop());
                });
                x.RunAsLocalSystem();

                x.SetDescription("Log Service proudly hosted by TopShelf");
                x.SetDisplayName("Log Service to Log");
                x.SetServiceName("LogService");
            });
        }

        public interface ILogService
        {
            void Start();
            void Stop();
        }

        public class LogService : ILogService
        {
            public void Start()
            {
                log.Debug("The TopShelf background thing actually worked!");
            }

            public void Stop()
            {
                log.Debug("The background thing stopped!");
            }
        }
    }


}
