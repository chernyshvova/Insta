using System;
using System.IO;
using System.Threading.Tasks;
using InstarmCore;
using InstarmCore.Database;
namespace InstarmConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Init();
                CommandLine cmd = new CommandLine();
                cmd.WriteHelp();
            }
            if (args.Length != 0)
            { 
                 RunAsync(args);
            }
            Console.ReadKey();
        }

        public static void RunAsync(string[] args)
        {
            try
            {
                var result = Task.Run(() => MainAsync(args));
                result.Wait();
                Console.WriteLine("========== END ==========");
            }
            catch (Exception ex)
            {
                var state = InstarmCore.Utils.ExeptionUtils.GetState();
                string err = InstarmCore.Utils.ExeptionUtils.ErrorMessage;
                Console.WriteLine("Error!:  " + ex);
            }
        }

        public static async Task<bool> MainAsync(string[] args)
        {
            CommandLine cmd = new CommandLine();
            await cmd.Execute(args);
            return false;
        }
        public static void Init()
        {
            if (!File.Exists(Environment.CurrentDirectory + @"\" + DbContract.DATABASE))
            {
                DbHelperSQLite dbhelper = new DbHelperSQLite();
                dbhelper.CreateTables();
            }
            if (!Directory.Exists(Environment.CurrentDirectory + PathContract.pathAccount))
            {
                Directory.CreateDirectory(Environment.CurrentDirectory + PathContract.pathAccount);
            }
            if (!Directory.Exists(Environment.CurrentDirectory + PathContract.pathAvatar))
            {
                Directory.CreateDirectory(Environment.CurrentDirectory + PathContract.pathAvatar);
            }
            if (!Directory.Exists(Environment.CurrentDirectory + PathContract.pathImg))
            {
                Directory.CreateDirectory(Environment.CurrentDirectory + PathContract.pathImg);
            }
        }
    }
}