using Domain.Core.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shell
{
    public interface IConsoleCommand : ICommand
    {
        string[] Keys { get; }
        string Usage { get; }

        void Build(string[] args);
    }
}
