using System;

namespace TalkNotes
{
    public class AnonObserver<T> : IObserver3<T>
    {
        private readonly Action<T> _onNext;
        private readonly Action<Exception> _onError;
        private readonly Action _onComplete;

        public AnonObserver(Action<T> onNext, Action<Exception> onError, Action onComplete)
        {
            _onNext = onNext;
            _onError = onError;
            _onComplete = onComplete;
        }

        public void OnNext(T value)
        {
            _onNext(value);
        }

        public void OnError(Exception e)
        {
            _onError(e);
        }

        public void OnComplete()
        {
            _onComplete();
        }
    }
}