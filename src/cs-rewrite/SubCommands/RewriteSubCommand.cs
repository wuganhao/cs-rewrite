using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WuGanhao.CommandLineParser;

namespace WuGanhao.CSharpRewrite.SubCommands {
    public abstract class RewriteSubCommand : SubCommand<CommandExecutor> {
        public async override Task Run() {
            IEnumerable<Task> tasks = this.FindFiles(this.Parent.Directory)
                .Select(file => this.Process(file, this.Parent.Interactive));

            await Task.WhenAll(tasks.ToArray());
        }

        protected abstract Task Process(string filePath, bool interactive = false);
        protected abstract IEnumerable<string> FindFiles(string path);

        protected virtual IEnumerable<string> FindFiles(string path, params string[] patterns) {
            string exclude = this.Parent.ExcludeDirectories ?? string.Empty;
            string[] excludes = exclude.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

            IEnumerable<string> currentFolderFiles = patterns
                .SelectMany(pattern => Directory.EnumerateFiles(path, pattern, SearchOption.TopDirectoryOnly))
                .Distinct();

            foreach (string file in currentFolderFiles) {
                yield return file;
            }

            foreach (string dir in Directory.EnumerateDirectories(path)) {
                string folderName = Path.GetFileName(dir);

                if (excludes.Any(e => e == folderName)) continue;

                foreach (string f in this.FindFiles(dir)) {
                    yield return f;
                }
            }
        }
    }
}
