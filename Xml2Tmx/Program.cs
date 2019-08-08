using ManyConsole;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xml2Tmx
{
    public class Program
    {
        public static int Main(string[] args)
        {
            var commands = GetCommands();

            return ConsoleCommandDispatcher.DispatchCommand(commands, args, Console.Out);
        }

        public static IEnumerable<ConsoleCommand> GetCommands()
        {
            return ConsoleCommandDispatcher.FindCommandsInSameAssemblyAs(typeof(Program));
        }

        // ConvertToXml -i "D:\Work\Aerofly\C208\Project\c208_backup\c208.tmd" -o "D:\test.xml"
        // ConvertToTmx -i "D:\test.xml" -o "D:\test.tmd"
    }
}
