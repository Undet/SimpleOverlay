using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POESimpleOverlay.Overlay
{
    internal interface IOverlay
    {
        public void Dispose();
        public void Run();
        public void ChangeVisability();
    }
}
