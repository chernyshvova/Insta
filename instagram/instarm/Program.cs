using System;
using System.Threading.Tasks;

namespace instarm
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                CommandLine cmd = new CommandLine();
                cmd.WriteHelp();
            }
            RunAsync(args);
            Console.ReadKey();
        }

        public static void RunAsync(string[] args)
        {      
            try
            {
                var result = Task.Run(() => MainAsync(args));
            }
            catch (Exception ex){

                Console.WriteLine("Error!:  " + ex);
            }
        }

        public static async Task<bool> MainAsync(string[] args)
        {        
            CommandLine cmd = new CommandLine();
            await cmd.Execute(args);
            return false;
        }
    }
}
