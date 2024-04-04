using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadsheetEngine {
    public abstract class Command {

        public abstract string Message { get; }
        public abstract void Execute();

        public abstract void Unexecute();

    }
}
