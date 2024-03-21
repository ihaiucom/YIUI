using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zeng.Mdx.Runtimes
{
    public interface IEmitterParameter
    {
        EmittedObjectUpdater emittedObjectUpdater { get; }
        float TimeScale { get; }
    }
}
