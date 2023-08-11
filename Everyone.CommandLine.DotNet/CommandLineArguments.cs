using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace everyone
{
    public class CommandLineArguments
    {
        private readonly List<CommandLineArgument> arguments;

        private CommandLineArguments()
        {
            this.arguments = new List<CommandLineArgument>();
        }

        public static CommandLineArguments Create()
        {
            return new CommandLineArguments();
        }

        public CommandLineArgument Get(int index)
        {
            return this.arguments[index];
        }

        public CommandLineArgument this[int index]
        {
            get { return this.Get(index); }
        }
    }
}
