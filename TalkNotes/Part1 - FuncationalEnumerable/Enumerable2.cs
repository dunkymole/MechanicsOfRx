using System;

namespace TalkNotes
{
    public static class Enumerable2
    {
        public interface IEnumerable2<out T>
        {
            IEnumerator2<T> GetEnumerator();
        }

        public interface IEnumerator2<out T> /*: IDisposable*/ //TODO come back to me!!! 
        {
            T Current { get; }
            bool MoveNext();
        }

        // Func<Func<Notification<T>>>

        public static Func<Func<Notification<int>>> Range(int start, int count)
        {
            return (() =>
                {
                    var i = 0;
                    return () => i < count ? Notification.Next(start + i++) : Notification.Complete<int>();
                });
        }

        public static Func<Func<Notification<char>>> Keys()
        {
            return (() =>
                {
                    Console.WriteLine("Press esc to exit");
                    return () =>
                        {
                            var k = Console.ReadKey(true);
                            return k.Key == ConsoleKey.Escape
                                       ? Notification.Complete<char>()
                                       : Notification.Next(k.KeyChar);
                        };
                });
        }

        public static Func<Func<Notification<T>>> Empty<T>()
        {
            return () => () => Notification.Complete<T>();
        }

        public static Func<Func<Notification<T>>> Return<T>(T value)
        {
            return () =>
                {
                    var first = true;
                    return () =>
                        {
                            if (first)
                            {
                                first = false;
                                return Notification.Next(value);
                            }
                            return Notification.Complete<T>();
                        };
                };
        }


        public static Func<Func<Notification<T>>> Where2<T>(this Func<Func<Notification<T>>> source,
                                                            Func<T, bool> predicate)
        {
            return source.SelectMany(v => predicate(v) ? Return(v) : Empty<T>(), (outer, inner) => inner);
        }

        public static Func<Func<Notification<T>>> Where<T>(this Func<Func<Notification<T>>> source,
                                                           Func<T, bool> predicate)
        {
            return () =>
                {
                    var enumerator2 = source();
                    return () =>
                        {
                            while (true)
                            {
                                var r = enumerator2();
                                if (r.NotificationType == NotificationType.Complete)
                                    return r;
                                if (predicate(r.Next))
                                    return Notification.Next(r.Next);
                            }
                        };
                };
        }

        public static Func<Func<Notification<TO>>> Select<TI, TO>(this Func<Func<Notification<TI>>> source,
                                                                  Func<TI, TO> projection)
        {
            return () =>
                {
                    var enumerator2 = source();
                    return () =>
                        {
                            var r = enumerator2();
                            return r.NotificationType == NotificationType.Complete
                                       ? Notification.Complete<TO>()
                                       : Notification.Next(projection(r.Next));
                        };
                };
        }

        public static Func<Func<Notification<TResult>>> SelectMany<TOuter, TInner, TResult>(
            this Func<Func<Notification<TOuter>>> source, Func<TOuter, Func<Func<Notification<TInner>>>> innerFactory,
            Func<TOuter, TInner, TResult> resultSelector)
        {
            return () =>
                {
                    var enumerator2 = source();
                    var stepOuter = true;
                    Func<Notification<TInner>> inner = null;
                    Notification<TOuter> currentOuter = null;
                    return () =>
                        {
                            while (true)
                            {
                                if (stepOuter)
                                {
                                    currentOuter = enumerator2();
                                    if (currentOuter.NotificationType == NotificationType.Complete)
                                        return Notification.Complete<TResult>();
                                    inner = innerFactory(currentOuter.Next)();
                                    stepOuter = false;
                                }
                                else
                                {
                                    var r = inner();
                                    if (r.NotificationType == NotificationType.Complete)
                                    {
                                        stepOuter = true;
                                        continue;
                                    }
                                    return Notification.Next(resultSelector(currentOuter.Next, r.Next));
                                }
                            }
                        };
                };
        }

        public static void ForEach<T>(this Func<Func<Notification<T>>> enumerable2, Action<T> action)
        {
            var enumerator2 = enumerable2();
            var r = enumerator2();
            while (r.NotificationType != NotificationType.Complete)
            {
                action(r.Next);
                r = enumerator2();
            }
        }
    }
}