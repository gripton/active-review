using System;
using NLog;



namespace ARR.Background
{
    class Program
    {
        private static readonly Logger log = LogManager.GetLogger(typeof(Program).Name);


        static void Main(string[] args)
        {

            Console.WriteLine("Starting - hit any key to continue...");

            log.Debug("The background thing actually worked!");
            
            Console.ReadLine();

            Console.WriteLine("Ending");
        }
    }
}
