using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleOverlay
{
    internal class SimpleHotKey : IDisposable
    {
        private readonly IntPtr _handle;

        private readonly int _id;

        private bool _isKeyRegistered;

        private Thread _disposer;

        public SimpleHotKey(ConsoleKey key, ConsoleModifiers modifiers, Action<SimpleHotKey> onPressed) : this(IntPtr.Zero, key, modifiers, onPressed) { }

        public SimpleHotKey(IntPtr wHandle,ConsoleKey key,ConsoleModifiers modifiers,  Action<SimpleHotKey> onPressed)
        {
            _id = GetHashCode();
            _handle = wHandle;
            this.Key = key;
            this.Modifiers = modifiers;

            if (onPressed != null)
            {
                SHKPressed += onPressed;
            }
            
        }

        public event Action<SimpleHotKey> SHKPressed;
        public ConsoleKey Key { get; private set; }
        public ConsoleModifiers Modifiers { get; private set; }




        public void Dispose()
        {
            throw new NotImplementedException();
        }

        private void OnHotKeyPresssed() 
        {
            SHKPressed?.Invoke(this);
        }
    }
}
