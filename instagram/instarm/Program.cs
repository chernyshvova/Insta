using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InstagramApiSharp.API;
using InstagramApiSharp.API.Builder;
using InstagramApiSharp.Classes;
using InstagramApiSharp.Logger;
using instarm.Database;

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
            int aaaaa = 44;//todo remove
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
            cmd.Execute(args);
            return false;
        }
    }
}
