using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace WuGanhao.CSharpRewrite.SubCommands {
    public class RemoveTrailingSpaces : RewriteSubCommand {

        protected override IEnumerable<string> FindFiles(string path) {
            string pattern = this.Parent.Pattern ?? "*.cs;*.txt;*.csproj;*.sln;*.xml;*.restext;*.resx;*.nuspec";
            string[] patterns = pattern.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            return this.FindFiles(path, patterns);
        }

        protected async override Task Process(string path, bool interactive = false) {
            try {
                bool fix = false;

                string[] lines = await File.ReadAllLinesAsync(path);

                fix = lines.Any(l => l.EndsWith(" ") || l.EndsWith("\t"));
                if (fix) {
                    Console.WriteLine($"  - {path}...");

                    IEnumerable<string> fixedLines = lines.Select(l => l.TrimEnd()).ToArray();

                    await File.WriteAllLinesAsync(path, fixedLines);
                }

            } catch (Exception ex) {
                Console.WriteLine($"Faile fixing {path} - {ex.Message}");
            }
        }
    }
}
