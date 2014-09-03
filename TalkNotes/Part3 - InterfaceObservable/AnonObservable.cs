using System;

namespace TalkNotes
{
    public class AnonObservable<T> : IObservable3<T>
    {
        private readonly Func<IObserver3<T>, IDisposable> _factory;

        public AnonObservable(Func<IObserver3<T>, IDisposable> factory)
        {
            _factory = factory;
        }

        public IDisposable Subscribe(IObserver3<T> observer)
        {
            return _factory(observer);
        }
    }
}