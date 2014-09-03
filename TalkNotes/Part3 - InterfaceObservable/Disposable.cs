using System;

namespace TalkNotes
{
    public static class Disposable
    {
        public static IDisposable Create(Action action)
        {
            return new AnonDisposable(action);
        }

        public static IDisposable Empty
        {
            get { return Create(() => { }); }
        }
    }
}