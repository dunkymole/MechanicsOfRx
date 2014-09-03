using System;
using System.Threading;

namespace TalkNotes
{
    public static class Observable2
    {
        /*
         * IEnumerable = Func<Func<Notification<int>>>
         * IObservable = Action<Action<Notification<int>>>
         * */

        public static Action<Action<Notification<int>>> Range(int start, int count)
        {
            Action<Action<Notification<int>>> observable = observer =>
                {
                    for (var i = 0; i < count; i++)
                        observer(Notification.Next(start + i));

                    observer(Notification.Complete<int>());
                };
            return observable;
        }

        public static Action<Action<Notification<char>>> Keys()
        {
            Action<Action<Notification<char>>> observable = observer =>
                {
                    Console.WriteLine("Press esc to exit");
                    while (true)
                    {
                        var k = Console.ReadKey(true);
                        if (k.Key == ConsoleKey.Escape)
                            break;
                        observer(Notification.Next(k.KeyChar));
                    }
                    observer(Notification.Complete<char>());
                };
            return observable;
        }

        public static Action<Action<Notification<int>>> Interval(TimeSpan timeout)
        {
            Action<Action<Notification<int>>> observable =
                observer =>
                {
                    var i = 0;
                    new Timer(_ => observer(Notification.Next(i++)), null, (long) timeout.TotalMilliseconds,
                        (long) timeout.TotalMilliseconds);
                };
            return observable;
        }

        public static Action<Action<Notification<int>>> Interval(TimeSpan timeout, int count)
        {
            Action<Action<Notification<int>>> observable = observer =>
                {
                    var i = 0;
                    Timer t = null;
                    t = new Timer(_ =>
                        {
                            observer(Notification.Next(i++));
                            if (i != count) return;
                            if (t != null) t.Dispose();
                            observer(Notification.Complete<int>());
                        }, null, (long) timeout.TotalMilliseconds, (long) timeout.TotalMilliseconds);
                };
            return observable;
        }

        public static void Subscribe<T>(this Action<Action<Notification<T>>> source, Action<T> onNext,
                                        Action<Exception> onError, Action onComplete)
        {
            Action<Notification<T>> observer = n =>
                {
                    switch (n.NotificationType)
                    {
                        case NotificationType.Next:
                            onNext(n.Next);
                            break;
                        case NotificationType.Complete:
                            onComplete();
                            break;
                        case NotificationType.Error:
                            onError(n.Exception);
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                };
            source(observer);
        }

        public static void ForEach<T>(this Action<Action<Notification<T>>> source, Action<T> onNext)
        {
            Action<Notification<T>> observer2 = n =>
                {
                    if (n.NotificationType == NotificationType.Next)
                        onNext(n.Next);
                    else
                        Console.WriteLine("done");
                };
            source(observer2);
        }
    }
}