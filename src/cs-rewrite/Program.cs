using System;
using System.ComponentModel;
using System.Threading.Tasks;
using WuGanhao.CommandLineParser;
using WuGanhao.CSharpRewrite.SubCommands;

namespace WuGanhao.CSharpRewrite {
    [SubCommand(typeof(RemoveTrailingSpaces), "remove-trailing-spaces", "Remove trailing spaces")]
    [Description("A tool for help rewrite C# code")]
    public class CommandExecutor {
        [CommandOption("dir", "d", "Directory for start code processing")]
        public string Directory { get; set; }

        [CommandOption("pattern", "p", "Search file pattern")]
        public string Pattern { get; set; }

        [CommandOption("exclude-directories", "e", "Directories excluded for processing, Seperated by comma")]
        public string ExcludeDirectories { get; set; } = "packages;obj;bin";

        [CommandSwitch("interactive", "i", "Allow command interaction")]
        public bool Interactive { get; set; }
    }

    public class Program {
        static async Task<int> Main(string[] args) {
            CommandLineParser<CommandExecutor> CmdLineParser = new CommandLineParser<CommandExecutor>();

            try {
                await CmdLineParser.Invoke();
                return 0;
            } catch (Exception ex) {
                System.Console.WriteLine(ex);
                return -1;
            }
        }
    }
}
