using System;
using System.Threading;

namespace TalkNotes
{
    public class AnonDisposable : IDisposable
    {
        private readonly Action _action;
        private int _isDisposed;

        public AnonDisposable(Action action)
        {
            _action = action;
        }

        public void Dispose()
        {
            if(Interlocked.CompareExchange(ref _isDisposed, 1, 0) ==0)
                _action();
        }
    }
}